import { NgModule } from '@angular/core';

import { AdminLayoutComponent } from './admin-layout.component';
import { AdminLayoutRouting } from './admin-layout.routing';
import { DashboardComponent } from './dashboard/dashboard.component';

@NgModule({
  declarations: [AdminLayoutComponent, DashboardComponent],
  imports: [AdminLayoutRouting],
  providers: []
})
export class AdminModule { }
