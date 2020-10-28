import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/shared/services/common.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  adults: number;
  dates: any;
  dateFrom: string;
  dateTo: string;
  constructor(private route: ActivatedRoute, private commonService: CommonService ) { }

  ngOnInit() {
    this.adults = this.route.snapshot.queryParams['Adults'];
    this.dateFrom = this.commonService.formatDate(this.route.snapshot.queryParams['DateFrom'], false);
    this.dateTo = this.commonService.formatDate(this.route.snapshot.queryParams['DateTo'], false);
  }

}
