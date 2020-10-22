import { Component, Input, OnInit } from '@angular/core';
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

  constructor() { }

  ngOnInit() {
  }

}
