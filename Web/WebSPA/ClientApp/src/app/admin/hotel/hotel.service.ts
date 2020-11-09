import { HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { PaginatedResult } from 'src/app/shared/model/pagination.model';
import { ConfigurationService } from 'src/app/shared/services/configuration.service';
import { DataService } from 'src/app/shared/services/data.service';
import { SecurityService } from 'src/app/shared/services/security.service';


@Injectable({
  providedIn: 'root'
})
export class HotelService {
  private hotelUrl: string = '';
  urlSuffix = '/api/v1/hotel-api/'; // '/api/v1/Hotel/';

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
    if (this.hotelUrl === '' || this.hotelUrl === null) {
      this.hotelUrl = 'http://localhost:8000';
    }
  }

  getHotelById(hotelId: number): Observable<Hotel> {
    let url = this.hotelUrl + this.urlSuffix + hotelId;

    // tslint:disable-next-line:max-line-length
    return this.service.getById(url)
    .pipe<Hotel>
      (tap((response: Hotel) => {
        return response;
      })
      );
  }

  getHotels(page?, itemsPerPage?, searchParams?): Observable<PaginatedResult<Hotel[]>> {
    let url = this.hotelUrl + this.urlSuffix + 'HotelsList';

    let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        if (searchParams != null) {
          // tslint:disable-next-line:max-line-length
          params = params.append('city', searchParams.City === 'undefined' ? null : searchParams.City);

          if (searchParams.Adults)
            {
              params = params.append('adults', searchParams.Adults || searchParams.adults);
            }
        }

    // tslint:disable-next-line:max-line-length
    return this.service.getAll(url, page, itemsPerPage, params).pipe<PaginatedResult<Hotel[]>>(tap((response: PaginatedResult<any[]>) => {
      return response;
    }));
  }

  searchHotels(page?, itemsPerPage?, searchParams?): Observable<PaginatedResult<Hotel[]>> {
    let url = this.hotelUrl + this.urlSuffix + 'SearchHotel';

    let params = new HttpParams();

        if (page != null && itemsPerPage != null) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        if (searchParams != null) {
            params = params.append('city', searchParams.City);
            params = params.append('adults', searchParams.Adults);
        }

    // tslint:disable-next-line:max-line-length
    return this.service.getAll(url, page, itemsPerPage, params).pipe<PaginatedResult<Hotel[]>>(tap((response: PaginatedResult<any[]>) => {
      return response;
    }));
  }

  delete(id: number): Observable<any> {
    let url = this.hotelUrl + this.urlSuffix + id;

    // tslint:disable-next-line:max-line-length
    return this.service.Delete(url)
    .pipe
        (tap((response: any) => response));

  }

  update(hotel: Hotel): Observable<boolean> {
    let url = this.hotelUrl + this.urlSuffix + 'Update';

    // tslint:disable-next-line:max-line-length
    return this.service.putWithId(url, hotel)
    .pipe
        (tap((response: any) => response));

  }

  saveHotel(hotel: Hotel): Observable<any> {
    let url = this.hotelUrl  + this.urlSuffix + 'Save';

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
