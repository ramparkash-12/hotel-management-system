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
  public dateFormat = 'yyyy-MM-dd';
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

  saveHotel(hotel: Hotel): Observable<any> {
    let url = 'http://localhost:2500' + this.urlSuffix;

    const formData = new FormData();
    formData.append('name', hotel.name);
    formData.append('description', hotel.description);
    formData.append('address', hotel.address);
    formData.append('country', hotel.country);
    formData.append('city', hotel.city);
    formData.append('status', hotel.status);
    formData.append('stars', hotel.stars.toString());
    formData.append('isFeatured', hotel.isFeatured);
    formData.append('featuredFrom', hotel.featuredFrom);
    formData.append('featuredTo', hotel.featuredTo);

    if (hotel.images.length > 0) {
      let i = 1;
      hotel.images.forEach(image => {
      formData.append('image-' + i  , image);
      i++;
      });
    }

    return this.service.postDataWithBlob(url, formData)
      .pipe
        (tap((response: any) => response));
  }
}
