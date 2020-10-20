import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../shared/guards/auth.guard';

import { AdminLayoutComponent } from './admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HotelAddComponent } from './hotel/hotel-add/hotel-add.component';
import { HotelEditResolver } from './hotel/hotel-edit.resolver';
import { HotelEditComponent } from './hotel/hotel-edit/hotel-edit.component';
import { HotelListComponent } from './hotel/hotel-list/hotel-list.component';

const adminRoutes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'dashboard',
        component: DashboardComponent
      },
      {
        path: 'hotel',
        component: HotelListComponent
      },
      {
        path: 'hotel/add',
        component: HotelAddComponent
      },
      {
        path: 'hotel/edit/:hotelId',
        component: HotelEditComponent,
        resolve: { hotel: HotelEditResolver }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminLayoutRouting { }
