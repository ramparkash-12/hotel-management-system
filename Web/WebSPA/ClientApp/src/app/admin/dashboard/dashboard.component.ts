import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/shared/services/alertify.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
  }

}
