using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users{userId}/[controller]")]
    [ApiController]
    public class MessagesController: ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController (IDatingRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;

        }

       [HttpGet("{id}", Name = "GetMessage")]
       public async Task<IActionResult> GetMessage(int userId, int id)
       {
           if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo == null)
                return NotFound();

            return Ok(messageFromRepo);
       }

       [HttpGet]
       public async Task<IActionResult> GetMessagesForUser(int userId,
                    [FromQuery]MessageParams messageParams)
       {
           if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);

            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
  
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize ,
                                   messagesFromRepo.TotalCount , messagesFromRepo.TotalPages);

            return Ok(messages);
       
       }

        [HttpGet("thread/{recepientId}")]
       public async Task<IActionResult> GetMessagesThread(int userId,
                    int recepientId)
       {
           if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

            var messagesFromRepo = await _repo.GetMessageThread(userId, recepientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            return Ok(messageThread);
       
       }

       [HttpPost]
       public async Task<IActionResult> CreateMessage(int userId, 
            MessageForCreationDto messageForCreationDto)
       {
           messageForCreationDto.SenderId = userId;

           var recepient = await _repo.GetUser(messageForCreationDto.RecepientId);

           if(recepient == null)
            return BadRequest("Could not find user");

            var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add(message);

            var massageToReturn = _mapper.Map<MessageForCreationDto>(message);

            if(await _repo.SaveAll())
                return CreatedAtRoute("GetMessage", new {id = message.Id, message});
       
            throw new Exception("Creating the message failed on save");
       }
    }
}