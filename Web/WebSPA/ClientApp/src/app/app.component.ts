import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertifyService } from './shared/services/alertify.service';
import { ConfigurationService } from './shared/services/configuration.service';
import { SecurityService } from './shared/services/security.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {
  title = 'app';
  authenticated: boolean = false;
  private subscription: Subscription;

  constructor(private alertify: AlertifyService, private securityService: SecurityService,
    private configurationService: ConfigurationService) { }

  ngOnInit() {
    this.subscription = this.securityService.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
  });

  if (window.location.hash) {
      this.securityService.AuthorizedCallback();
  }

    console.log('identity component, checking authorized: ' + this.securityService.IsAuthorized);
    this.authenticated = this.securityService.IsAuthorized;

    console.log('configuration');
    this.configurationService.load();
  }

}
