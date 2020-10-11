import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertifyService } from './shared/services/alertify.service';
import { SecurityService } from './shared/services/security.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';
  authenticated: boolean = false;
  private subscription: Subscription;
  userName: string = '';

  constructor(private alertify: AlertifyService, private securityService: SecurityService) { }

  ngOnInit() {
    this.subscription = this.securityService.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
      this.userName = this.securityService.UserData.first_name + ' ' + this.securityService.UserData.last_name;
  });

  if (window.location.hash) {
      this.securityService.AuthorizedCallback();
  }

  console.log('identity component, checking authorized: ' + this.securityService.IsAuthorized);
  this.authenticated = this.securityService.IsAuthorized;

  if (this.authenticated) {
      if (this.securityService.UserData) {
        this.userName = this.securityService.UserData.first_name + ' ' + this.securityService.UserData.last_name;
      }
    }
  }

}
