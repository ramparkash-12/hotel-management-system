import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { SecurityService } from '../services/security.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private securityService: SecurityService, private alertify: AlertifyService,
              private route: Router) { }

  canActivate(): Promise<boolean> | boolean {
    return true;
    /*
    if (this.securityService.IsAuthorized) {
      console.log('URL: ' + this.route.url);
      if (this.securityService.UserData.role === 'Customer') {
        console.log('Role: ' + this.securityService.UserData.role);
        this.route.navigate(['/forbidden']);
        return false;
      }
        return true;
    }

    //this.alertify.error('You shall not pass - Please Login!!');
    this.securityService.Authorize();
    return false;
    */
  }

}
