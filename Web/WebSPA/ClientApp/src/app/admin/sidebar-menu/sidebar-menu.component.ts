import { Component, OnInit } from '@angular/core';
import { SecurityService } from 'src/app/shared/services/security.service';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {

  constructor(private securityService: SecurityService) { }

  ngOnInit() {
  }

  sampleCall() {
    this.securityService.SampleCall();
  }

}
