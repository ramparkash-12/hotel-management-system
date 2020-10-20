import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable()
export class CommonService {
  public ServerdateFormat = 'yyyy-MM-dd';
  public ClientdateFormat = 'dd-MM-yyyy';

  constructor(private datepipe: DatePipe) {

  }

  formatDate(date: string, convertDateForClient: boolean) {

    if (convertDateForClient === false) {
      date = moment(date, 'DD/MM/YYYY').toString();
    }

    return this.datepipe.transform(date, convertDateForClient === true ? this.ClientdateFormat : this.ServerdateFormat, null, 'en-GB');
  }
}
