import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse, HttpHeaderResponse, HttpParams } from "@angular/common/http";

import { Observable, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { SecurityService } from './security.service';
import { Guid } from '../../../guid';
import { error } from 'protractor';
import { PaginatedResult } from '../model/pagination.model';

@Injectable()
export class DataService {
    options: any;
    constructor(private http: HttpClient, private securityService: SecurityService) { }

    getById(url: string): Observable<any> {
        this.options = { };
        this.setHeaders(this.options);

        return this.http.get<any>(url , { headers: this.options  })
        .pipe(
            tap((res: any) => {
                return res;
            }),
            catchError(this.handleError)
          );
    }


    getAll(url: string, page?, itemsPerPage?, searchParams?: any): Observable<PaginatedResult<any []>> {
        const paginatedResult: PaginatedResult<any[]> = new PaginatedResult<any[]>();
        // tslint:disable-next-line:prefer-const
        this.options = { };
        this.setHeaders(this.options);

        return this.http.get<any[]>(url, {headers: this.options, observe: 'response', params: searchParams })
          .pipe(
            map(response => {
                console.log(response);
              paginatedResult.result = response.body;
              if (response.headers.get('Pagination') != null) {
                paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
              }
              return paginatedResult;
            }),
            catchError(this.handleError)
          );
    }

    postWithId(url: string, data: any, params?: any): Observable<Response> {
        return this.doPost(url, data, true, params);
    }

    postDataWithBlob(url: string, data: any, params?: any): Observable<any> {
        return this.doPostWithBlob(url, data, false, params);
    }

    post(url: string, data: any, params?: any): Observable<Response> {
        return this.doPost(url, data, false, params);
    }

    putWithId(url: string, data: any, params?: any): Observable<Response> {
        return this.doPut(url, data, true, params);
    }

    private doPostWithBlob(url: string, data: any, needId: boolean, params?: any): Observable<any> {
        // tslint:disable-next-line: prefer-const
        this.options = { };
        this.setHeaders(this.options, needId);

        return this.http.post(url, data,
            { reportProgress: true,
                headers: this.options,
                observe: 'events'
            })
        .pipe(
            tap((res: any) => {
                return res;
            }),
            catchError(this.handleError)
        );
    }

    private doPost(url: string, data: any, needId: boolean, params?: any): Observable<Response> {
        // tslint:disable-next-line: prefer-const
        this.options = { };
        this.setHeaders(this.options, needId);

        return this.http.post(url, data, { headers: this.options } )
            .pipe(
                tap((res: Response) => {
                    return res;
                }),
                catchError(this.handleError)
            );
    }

    Delete(url: string, id?: number): Observable<any> {
        this.options = { };
        this.setHeaders(this.options);

        return this.http.delete(url, { headers: this.options})
        .pipe(
            tap((res: any) => {
                return res;
            }),
            catchError(this.handleError)
        );
    }

    private handleError(error: any) {
        if (error.error instanceof ErrorEvent) {
            // A client-side or network error occurred. Handle it accordingly.
            console.error('Client side network error occurred:', error.error.message);
        }
        // Unauthorized
        if (error.status === 401) {
            console.log('401');
            return throwError(error.statusText);
          }
          // Not supported
          if (error.status === 403) {
            console.log('403');
            return throwError(error.statusText);
          }
          // Custom Application Error returned by server
          const applicationError = error.headers.get('Application-Error');
          if (applicationError) {
            console.error('Application-Error' + applicationError);
            return throwError(applicationError);
          }
          // Other user generated errors
          const serverError = error.error;
          let modalStateErrors = '';
          if (serverError && typeof serverError === 'object') {
            for (const key in serverError) {
              if (serverError[key]) {
                modalStateErrors += serverError[key] + '\n';
              }
            }
            console.error('Backend - ' +
            `status: ${error.status}, ` +
            `statusText: ${error.statusText}, ` +
            `message: ${error.error.message}`);
          } else {
            console.error('Backend - ' +
                `status: ${error.status}, ` +
                `statusText: ${error.statusText}, ` +
                `message: ${error.error}`);
          }
          // return a user-facing error message
        return throwError(modalStateErrors || serverError || 'Server Error');
    }

    private doPut(url: string, data: any, needId: boolean, params?: any): Observable<Response> {
        this.options = { };
        this.setHeaders(this.options, needId);

        return this.http.put(url, data, { headers: this.options })
            .pipe(
                tap((res: Response) => {
                    return res;
                }),
                catchError(this.handleError)
            );
    }

    private setHeaders(options: any, needId?: boolean) {
        if (needId && this.securityService) {
            /*
            options['headers'] = new HttpHeaders()
                .append('authorization', 'Bearer ' + this.securityService.GetToken())
                .append('x-requestid', Guid.newGuid());
            */
                // tslint:disable-next-line:max-line-length
                this.options = new HttpHeaders({'authorization' : 'Bearer ' + this.securityService.GetToken(), 'x-requestid' : Guid.newGuid()});
        } else if (this.securityService) {
            this.options = new HttpHeaders({'authorization' : 'Bearer ' + this.securityService.GetToken() });
            //options['headers'] = new HttpHeaders()
                //.append('authorization', 'Bearer ' + this.securityService.GetToken());
        }
    }
}
