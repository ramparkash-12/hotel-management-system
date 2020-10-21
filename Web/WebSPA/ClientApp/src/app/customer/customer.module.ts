import { NgModule } from '@angular/core';
import { CustomerLayoutComponent } from './customer-layout.component';
import { CustomerLayoutRouting } from './customer-layout.routing';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { HomeSearchComponent } from './home/search/search.component';
import { SecurityService } from '../shared/services/security.service';
import { CommonModule, DatePipe } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SidebarComponent } from './search/sidebar/sidebar.component';
import { SearchHotelItemComponent } from './search/search-hotel-item/search-hotel-item.component';
import { SearchComponent } from './search/search.component';
import { HotelService } from '../admin/hotel/hotel.service';
import { DataService } from '../shared/services/data.service';
import { CommonService } from '../shared/services/common.service';
import { SearchHotelItemDetailComponent } from './search/search-hotel-item-detail/search-hotel-item-detail.component';
import { NgxGalleryModule } from 'ngx-gallery';


@NgModule({
  declarations: [ CustomerLayoutComponent, HomeComponent, NavMenuComponent, FooterComponent,
     HomeSearchComponent,
     SearchComponent,
     SidebarComponent, SearchHotelItemComponent, SearchHotelItemDetailComponent],
  imports: [
    CustomerLayoutRouting,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxGalleryModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    SweetAlert2Module.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    TypeaheadModule.forRoot()
  ],
  providers: [
    HotelService,
    DataService,
    CommonService,
    DatePipe,

  ]
})
export class CustomerModule { }
