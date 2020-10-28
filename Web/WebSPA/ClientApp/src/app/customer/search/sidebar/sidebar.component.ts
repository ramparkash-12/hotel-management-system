import { Component, Input, OnInit } from '@angular/core';
import { CommonService } from 'src/app/shared/services/common.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  @Input() queryParams: any = { };
  searchCriteria: any = {
    City: '',
    Adults: '',
    Dates: []
  };

  convertDatesString: string;

  selected: string;
  cities: string[] = [
    'California',
    'LA',
    'Dubai',
    'London'
  ];

  constructor(private commonService: CommonService) { }

  ngOnInit() {
    this.searchCriteria = this.queryParams[0];

    if (this.queryParams[0].Dates !== null && this.queryParams[0].Dates !== undefined) {
      this.convertDatesString = this.queryParams[0].Dates;
      this.searchCriteria.Dates[0] = (this.commonService.formatDate(this.convertDatesString.toString().split(',')[0], true));
      this.searchCriteria.Dates[1] = (this.commonService.formatDate(this.convertDatesString.toString().split(',')[1], true));
      console.log('datesArray: ' + this.searchCriteria.Dates);
    }

    console.log('criteria:' + this.searchCriteria.toString());

  }

  onSave() {
    console.log(this.searchCriteria);
  }

}
