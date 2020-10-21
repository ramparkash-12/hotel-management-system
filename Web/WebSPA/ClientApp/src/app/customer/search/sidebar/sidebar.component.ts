import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  selected: string;
  cities: string[] = [
    'California',
    'LA',
    'Dubai',
    'London'
  ];
  searchCriteria: any = {
    adults: 1
  } ;

  constructor() { }

  ngOnInit() {
  }

  onSave() {
    console.log(this.searchCriteria);
  }

}
