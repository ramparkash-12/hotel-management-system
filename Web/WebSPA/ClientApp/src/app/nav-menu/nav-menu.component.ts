import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SecurityService } from '../shared/services/security.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  authenticated: boolean = false;
  private subscription: Subscription;
  userName: string = '';


  constructor(private service: SecurityService) {  }

  ngOnInit() {
    this.subscription = this.service.authenticationChallenge$.subscribe(res => {
      this.authenticated = res;
      this.userName = this.service.UserData.first_name + ' ' + this.service.UserData.last_name;
  });

  if (window.location.hash) {
      this.service.AuthorizedCallback();
  }

  console.log('identity component, checking authorized: ' + this.service.IsAuthorized);
  this.authenticated = this.service.IsAuthorized;

  if (this.authenticated) {
      if (this.service.UserData) {
        this.userName = this.service.UserData.first_name + ' ' + this.service.UserData.last_name;
      }
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  login() {
    this.service.Authorize();
  }
}
