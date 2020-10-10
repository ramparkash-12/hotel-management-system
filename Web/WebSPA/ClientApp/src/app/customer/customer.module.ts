import { NgModule } from '@angular/core';
import { CustomerLayoutComponent } from './customer-layout.component';
import { CustomerLayoutRouting } from './customer-layout.routing';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FooterComponent } from './footer/footer.component';
import { SearchComponent } from './search/search.component';

@NgModule({
  declarations: [ CustomerLayoutComponent, HomeComponent, NavMenuComponent, FooterComponent, SearchComponent],
  imports: [CustomerLayoutRouting]
})
export class CustomerModule { }
