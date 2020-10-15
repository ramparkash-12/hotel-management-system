import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { DataService } from 'src/app/shared/services/data.service';
import { SecurityService } from 'src/app/shared/services/security.service';


@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private hotelUrl: string = '';
  urlSuffix = '/api/v1/Hotel/Save'; //'/api/v1/hotel-api/Save';

  constructor(private service: DataService, private authService: SecurityService,
    private configurationService: ConfigurationService) {
      if (this.authService.IsAuthorized) {
        if (this.authService.UserData) {
            if (this.configurationService.isReady) {
                this.hotelUrl = this.configurationService.serverSettings.hotelUrl;
            } else {
                this.configurationService.settingsLoaded$.subscribe(x => {
                  this.hotelUrl = this.configurationService.serverSettings.hotelUrl;
                });
            }
        }
    }
  }

  saveHotel(hotel: Hotel): Observable<boolean> {
    let url = 'http://localhost:2500' + this.urlSuffix;
    return this.service.post(url, hotel)
      .pipe<boolean>
        (tap((response: any) => true));
  }
}
