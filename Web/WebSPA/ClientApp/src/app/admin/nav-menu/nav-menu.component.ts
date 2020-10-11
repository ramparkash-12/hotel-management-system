import { Component, OnInit } from '@angular/core';
import { SecurityService } from 'src/app/shared/services/security.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor(private securityService: SecurityService) { }

  ngOnInit() {
  }

  logout() {
    this.securityService.Logoff();
}

}
