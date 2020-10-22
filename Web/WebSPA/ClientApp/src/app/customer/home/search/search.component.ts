import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HotelService } from 'src/app/admin/hotel/hotel.service';

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

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onSave() {
    this.router.navigate(['/search'],
    { queryParams: {
      City: this.searchCriteria.city,
      Adults: this.searchCriteria.adults
    }
  });
  }

}
