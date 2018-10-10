using System;

namespace DatingApp.API.Models.Dtos
{
    public class MessageForCreationDto
    {
        public int SenderId { get; set; }
        public int RecepientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public string SenderPhotoUrl { get; set; }

         public string RecipientPhotoUrl { get; set; }


        public  MessageForCreationDto() 
        {
            MessageSent = DateTime.Now;
        }
    }
}