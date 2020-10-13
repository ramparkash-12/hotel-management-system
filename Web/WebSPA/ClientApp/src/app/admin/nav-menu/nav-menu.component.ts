import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SecurityService } from 'src/app/shared/services/security.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  authenticated: boolean = false;
  private subscription: Subscription;
  userName: string = '';

  constructor(private securityService: SecurityService) { }

  ngOnInit() {
    this.subscription = this.securityService.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
      this.userName = this.securityService.UserData.first_name + ' ' + this.securityService.UserData.last_name;
  });

  if (this.securityService.IsAuthorized) {
    if (this.securityService.UserData) {
      this.userName = this.securityService.UserData.first_name + ' ' + this.securityService.UserData.last_name;
    }
    this.authenticated = true;
  }
  }

  logout() {
    this.securityService.Logoff();
}

}
