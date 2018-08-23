import { Injectable } from '@angular/core';
// import * as alertify from 'alertify.js'; // import
// import alertify from 'node_modules/alertifyjs';
// import { alertify } from 'alertifyjs';
 declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

  constructor() { }

  confirm(message: string, okCallback: () => any) {
    alertify.confirm(message, function(e) {
        if (e) {
          okCallback();
        } else {}
    });
  }

  success(message: string) {
    alertify.success(message);
  }

  erorr(message: string) {
    alertify.succsess(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }
}
