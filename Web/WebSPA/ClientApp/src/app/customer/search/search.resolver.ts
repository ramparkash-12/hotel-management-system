import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Resolve, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HotelService } from 'src/app/admin/hotel/hotel.service';

import { Hotel } from 'src/app/shared/model/hotel.model';
import { PaginatedResult } from 'src/app/shared/model/pagination.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class SearchResolver implements Resolve<Hotel[]> {
  pageNumber = 1;
  pageSize = 10;

  // tslint:disable-next-line:max-line-length
  constructor(private service: HotelService, private alertify: AlertifyService,
    private router: Router) { }

  resolve(routeParams: ActivatedRouteSnapshot): Observable<Hotel[]>  {
    let searchParams: any = { };
    searchParams.City = routeParams.queryParams['City'];
    searchParams.Adults = routeParams.queryParams['Adults'];

    return this.service.searchHotels(this.pageNumber, this.pageSize, searchParams).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving your data....!!');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
