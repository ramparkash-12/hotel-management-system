import { NgModule } from '@angular/core';

import { AdminLayoutComponent } from './admin-layout.component';
import { AdminLayoutRouting } from './admin-layout.routing';
import { DashboardComponent } from './dashboard/dashboard.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { SidebarMenuComponent } from './sidebar-menu/sidebar-menu.component';
import { FooterComponent } from './footer/footer.component';
import { HotelListComponent } from './hotel/hotel-list/hotel-list.component';
import { HotelItemComponent } from './hotel/hotel-list/hotel-item/hotel-item.component';
import { HotelAddComponent } from './hotel/hotel-add/hotel-add.component';
import { HotelEditComponent } from './hotel/hotel-edit/hotel-edit.component';
import { CommonModule } from '@angular/common';
import { NgxDropzoneModule } from 'ngx-dropzone';
import { NgxSummernoteModule } from 'ngx-summernote';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { HotelService } from './hotel/hotel.service';
import { DataService } from '../shared/services/data.service';

@NgModule({
  declarations: [
    AdminLayoutComponent,
    DashboardComponent, NavMenuComponent,
    SidebarMenuComponent, FooterComponent,
    HotelListComponent, HotelItemComponent, HotelAddComponent, HotelEditComponent
  ],
  imports: [
    AdminLayoutRouting,
    CommonModule,
    NgxDropzoneModule,
    NgxSummernoteModule,
    FormsModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
  ],
  providers: [
    HotelService,
    DataService
  ]
})
export class AdminModule { }
