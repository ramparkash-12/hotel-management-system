import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

import { CustomerLayoutComponent } from './customer-layout.component';
import { SearchComponent } from './search/search.component';
import { SearchHotelItemComponent } from './search/search-hotel-item/search-hotel-item.component';
import { SearchHotelItemDetailComponent } from './search/search-hotel-item-detail/search-hotel-item-detail.component';
import { SearchResolver } from './search/search.resolver';

const customerRoutes: Routes = [
  {
    path: '',
    component: CustomerLayoutComponent,
    children: [
      {
        path: 'home',
        component: HomeComponent
      },
      {
        path: 'search',
        component: SearchComponent,
        resolve: { hotels: SearchResolver }
      },
      {
        path: 'detail/:id',
        component: SearchHotelItemDetailComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(customerRoutes)],
  exports: [RouterModule],
})
export class CustomerLayoutRouting { }
