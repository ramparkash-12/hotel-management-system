import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ForbiddenComponent } from './shared/components/forbidden/forbidden.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';

export const routes: Routes = [
  { path: 'my', component: AppComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  {
    path: '',
    loadChildren: './customer/customer.module#CustomerModule'
  },
  {
    path: 'auth',
    loadChildren: './auth/auth.module#AuthModule'
  },
  {
    path: 'admin',
    loadChildren: './admin/admin.module#AdminModule'
  },
  { path: 'forbidden', component: ForbiddenComponent },
  { path: '**', component: PageNotFoundComponent }
];

export const routing = RouterModule.forRoot(routes);
