import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home--search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class HomeSearchComponent implements OnInit {
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
