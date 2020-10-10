import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AdminLayoutComponent } from './admin-layout.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { HotelAddComponent } from './hotel/hotel-add/hotel-add.component';
import { HotelEditComponent } from './hotel/hotel-edit/hotel-edit.component';
import { HotelListComponent } from './hotel/hotel-list/hotel-list.component';

const adminRoutes: Routes = [
  {
    path: '',
    component: AdminLayoutComponent,
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
        path: 'hotel/edit',
        component: HotelEditComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(adminRoutes)],
  exports: [RouterModule]
})
export class AdminLayoutRouting { }
