import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { HotelService } from './hotel.service';


@Injectable({
  providedIn: 'root'
})
export class HotelEditResolver implements Resolve<Hotel> {
  // tslint:disable-next-line:max-line-length
  constructor(private service: HotelService, private alertify: AlertifyService,
    private router: Router) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Hotel> {
    return this.service.getHotelById(route.params['hotelId']).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving your data....!!');
        this.router.navigate(['/admin/hotel']);
        return of(null);
      })
    );
  }
}