import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HotelService } from 'src/app/admin/hotel/hotel.service';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { CommonService } from 'src/app/shared/services/common.service';

@Component({
  selector: 'app-search-hotel-item',
  templateUrl: './search-hotel-item.component.html',
  styleUrls: ['./search-hotel-item.component.css']
})
export class SearchHotelItemComponent implements OnInit {
  @Input() hotel: Hotel;
  renderedHtmlDescription: any;
  @ViewChild('dataContainer', null) dataContainer: ElementRef;


  constructor() { }

  ngOnInit() {
  }

  onRenderedHtmlChange(data) {
    this.dataContainer.nativeElement.innerHTML = data;
    this.renderedHtmlDescription = this.dataContainer.nativeElement.innerText.substr(0, 200) + '.....';
    return this.renderedHtmlDescription;
  }

}
