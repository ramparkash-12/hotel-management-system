import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

import { IBooking } from 'src/app/shared/model/booking.model';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { DataService } from 'src/app/shared/services/data.service';
import { SecurityService } from 'src/app/shared/services/security.service';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private bookUrl: string = '';
  urlSuffix = '/api/v1/booking-api/'; // '/api/v1/Booking/';

  constructor(private service: DataService, private authService: SecurityService,
    private configurationService: ConfigurationService) {
      if (this.authService.IsAuthorized) {
        if (this.authService.UserData) {
            if (this.configurationService.isReady) {
                this.bookUrl = this.configurationService.serverSettings.bookingUrl;
            } else {
                this.configurationService.settingsLoaded$.subscribe(x => {
                  this.bookUrl = this.configurationService.serverSettings.bookingUrl;
                });
            }
        }
    }
    if (this.bookUrl === '' || this.bookUrl === null) {
      //this.bookUrl = 'http://localhost:2000';
      this.bookUrl = 'http://localhost:8000';
    }
  }

  save(booking: IBooking): Observable<boolean> {
    let url = this.bookUrl + this.urlSuffix + 'Save';

    // tslint:disable-next-line:max-line-length
    return this.service.postWithId(url, booking)
    .pipe
        (tap((response: any) => response));

  }

}
