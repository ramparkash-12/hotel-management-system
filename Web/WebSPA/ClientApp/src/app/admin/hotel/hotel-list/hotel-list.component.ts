import { Component, OnInit, ViewChild } from '@angular/core';
import { SwalComponent } from '@sweetalert2/ngx-sweetalert2';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { PaginatedResult, Pagination } from 'src/app/shared/model/pagination.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { SecurityService } from 'src/app/shared/services/security.service';
import { HotelService } from '../hotel.service';

@Component({
  selector: 'app-hotel-list',
  templateUrl: './hotel-list.component.html',
  styleUrls: ['./hotel-list.component.css']
})
export class HotelListComponent implements OnInit {
  loading = false;
  errorReceived: boolean;
  hotels: Hotel [];
  pagination: Pagination;
  paginationText: string;
  @ViewChild('confirmDeleted', null) private mySwal: SwalComponent;

  // tslint:disable-next-line:max-line-length
  constructor(private service: HotelService, private configurationService: ConfigurationService, private securityService: SecurityService, private alertify: AlertifyService,) { }

  ngOnInit() {
    if (this.configurationService.isReady) {
        this.loadData();
    } else {
        this.configurationService.settingsLoaded$.subscribe(x => {
            this.loadData();
        });
    }
  }

  onPageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getHotels(this.pagination.itemsPerPage, this.pagination.currentPage);
  }

  loadData() {
    this.loading = true;
    this.getHotels(10, 1);
  }

  getHotels(pageSize?: number, pageIndex?: number) {
    this.loading = true;
    this.errorReceived = false;
    this.service.getHotels(pageIndex, pageSize)
    .subscribe(
      (res: PaginatedResult<Hotel[]>) => {
      this.hotels = res.result;
      this.pagination = res.pagination;
    },
    error => {
      this.errorReceived = true;
    }, () => {
      this.loading = false;
      this.setPaginationText();
    }
    );
  }

  delete(id: number) {
    this.service.delete(id).subscribe(() => {
      this.hotels.splice(this.hotels.findIndex(h => h.id === id), 1);
      this.mySwal.fire();
    }, error => {
      this.alertify.error('Failed to delete the hotel');
    });
  }

  setPaginationText() {
    // tslint:disable-next-line:max-line-length
    this.paginationText = 'Showing ' + this.pagination.currentPage + ' of ' + Math.ceil(this.pagination.totalItems /  this.pagination.itemsPerPage) + ' pages';
    return this.paginationText;
  }

}
