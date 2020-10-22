import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HotelService } from 'src/app/admin/hotel/hotel.service';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { PaginatedResult, Pagination } from 'src/app/shared/model/pagination.model';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  hotels: Hotel[];
  pagination: Pagination;
  paginationText: string;
  searchCriteria: any = {  };
  constructor(private route: ActivatedRoute, private service: HotelService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.hotels = data['hotels'].result;
      this.pagination = data['hotels'].pagination;
    });

    this.searchCriteria = {
      City : this.route.snapshot.queryParams['City'],
      Adults : this.route.snapshot.queryParams['Adults']
    };


    console.log(this.hotels);
    console.log('search module: ' + this.searchCriteria);
  }

  searchHotels(pageSize?: number, pageIndex?: number, searchParams?: any) {
    //this.loading = true;
    //this.errorReceived = false;
    this.service.searchHotels(pageIndex, pageSize, searchParams)
    .subscribe(
      (res: PaginatedResult<Hotel[]>) => {
      this.hotels = res.result;
      this.pagination = res.pagination;
    },
    error => {
      //this.errorReceived = true;
    }, () => {
      //this.loading = false;
      this.setPaginationText();
    }
    );
  }

  onPageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.searchHotels(this.pagination.itemsPerPage, this.pagination.currentPage, this.searchCriteria);
  }

  setPaginationText() {
    // tslint:disable-next-line:max-line-length
    this.paginationText = 'Showing ' + this.pagination.currentPage + ' of ' + Math.ceil(this.pagination.totalItems /  this.pagination.itemsPerPage) + ' pages';
    return this.paginationText;
  }

}
