import { Injectable } from '@angular/core';
declare let alertify: any;

alertify.defaults = {
  // notifier defaults
  notifier: {
    // auto-dismiss wait time (in seconds)
        delay: 5,
    // default position
        position: 'bottom-top',
    // adds a close button to notifier messages
        closeButton: false,
    },
};

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

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }

}
