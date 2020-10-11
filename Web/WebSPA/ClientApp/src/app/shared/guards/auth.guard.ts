import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { SecurityService } from '../services/security.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private securityService: SecurityService, private alertify: AlertifyService) { }

  canActivate(): Promise<boolean> | boolean {
    if (this.securityService.IsAuthorized) {
      return true;
    }

    this.alertify.error('You shall not pass - Please Login!!');
    this.securityService.Authorize();
    return false;

  }

}
