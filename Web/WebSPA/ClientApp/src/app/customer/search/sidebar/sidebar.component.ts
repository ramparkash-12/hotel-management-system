import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() queryParams: any = { };
  searchCriteria: any = {
    City: '',
    Adults: ''
  };

  selected: string;
  cities: string[] = [
    'California',
    'LA',
    'Dubai',
    'London'
  ];

  constructor() { }

  ngOnInit() {
    this.searchCriteria = this.queryParams[0];
  }

  onSave() {
    console.log(this.queryParams);
  }

}
