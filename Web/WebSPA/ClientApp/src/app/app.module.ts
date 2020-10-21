import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppComponent } from './app.component';
import { PageNotFoundComponent } from './shared/components/page-not-found/page-not-found.component';
import { routes, routing } from './app.routes';
import { SecurityService } from './shared/services/security.service';
import { ConfigurationService } from './shared/services/configuration.service';
import { StorageService } from './shared/services/storage.service';
import { AuthGuard } from './shared/guards/auth.guard';
import { AlertifyService } from './shared/services/alertify.service';
import { ErrorInterceptorProvide } from './shared/services/error.interceptor';
import { ForbiddenComponent } from './shared/components/forbidden/forbidden.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { RouterModule } from '@angular/router';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CommonService } from './shared/services/common.service';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';

@NgModule({
  declarations: [
    AppComponent,
    PageNotFoundComponent,
    ForbiddenComponent
  ],
  imports: [
    //BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    routing,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    SweetAlert2Module.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    TypeaheadModule.forRoot()
    ],
  providers: [
    AuthGuard,
    //ErrorInterceptorProvide,
    AlertifyService,
    SecurityService,
    CommonService,
    ConfigurationService,
    StorageService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
