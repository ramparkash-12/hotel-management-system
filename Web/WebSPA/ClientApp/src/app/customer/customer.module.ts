import { NgModule } from '@angular/core';
import { CustomerLayoutComponent } from './customer-layout.component';
import { CustomerLayoutRouting } from './customer-layout.routing';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [ CustomerLayoutComponent, HomeComponent],
  imports: [CustomerLayoutRouting]
})
export class CustomerModule { }
