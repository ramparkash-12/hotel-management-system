import { DatePipe, formatDate } from '@angular/common';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { CommonService } from 'src/app/shared/services/common.service';
import { HotelService } from '../hotel.service';
import * as moment from 'moment';

@Component({
  selector: 'app-hotel-edit',
  templateUrl: './hotel-edit.component.html',
  styleUrls: ['./hotel-edit.component.css']
})
export class HotelEditComponent implements OnInit {
  files: File[] = [];
  progress = 0;
  size = 0;
  unit = '';
  content: any;
  config = {
    placeholder: 'Add Hotel Description....',
    tabsize: 2,
    height: '200px',
    toolbar: [
        ['misc', ['codeview', 'undo', 'redo']],
        ['style', ['bold', 'italic', 'underline', 'clear']],
        ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'clear']],
        ['fontsize', ['fontname', 'fontsize', 'color']],
        ['para', ['style', 'ul', 'ol', 'paragraph', 'height']]
    ],
    fontNames: ['Helvetica', 'Arial', 'Arial Black', 'Comic Sans MS', 'Courier New', 'Roboto', 'Times']
  };
  hotelForm: FormGroup;
  formSubmitted: Boolean = false;
  hotel: Hotel;
  loading = false;

  constructor(private fb: FormBuilder, private alertify: AlertifyService,
    private hotelService: HotelService, public datepipe: DatePipe,
    private route: ActivatedRoute, private commonService: CommonService) { }
    @ViewChild('featuredToDate', null) featuredToDate: ElementRef;

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.hotel = data['hotel'];
    });
    this.createHotelForm();
  }

  createHotelForm() {
    this.hotelForm = this.fb.group({
      id: [this.hotel.id],
      name: [this.hotel.name, [Validators.required, Validators.maxLength(100)]],
      description: [this.hotel.description, Validators.required],
      address: [this.hotel.address, Validators.required],
      country: [this.hotel.country, Validators.required],
      city: [this.hotel.city, Validators.required],
      status: [this.hotel.status],
      stars: [this.hotel.stars],
      isFeatured: [this.hotel.isFeatured, Validators.required],
      featuredFrom: '',
      featuredTo: '',
      images: ['']
    });

    if (Boolean(this.hotel.isFeatured) === true) {
      if (this.hotel.featuredFrom !== '0001-01-01T00:00:00') {
        this.hotel.featuredFrom = this.commonService.formatDate(this.hotel.featuredFrom, true);
        this.hotelForm.get('featuredFrom').setValue(this.hotel.featuredFrom);
      }
      if (this.hotel.featuredTo !== '0001-01-01T00:00:00') {
        this.hotel.featuredTo = this.commonService.formatDate(this.hotel.featuredTo, true);
        //this.hotel.featuredTo = new Date(this.hotel.featuredTo).toString();
        this.hotelForm.get('featuredTo').setValue(this.hotel.featuredTo);
      }
    } else {
      this.onFeaturedChange('false');
    }
    if (this.hotel.images.length > 0) {
      this.files = this.hotel.images;
    }
    console.log(this.hotel);
  }

  onSelect(event) {
    this.files.push(...event.addedFiles);
  }

  onRemove(event) {
    this.files.splice(this.files.indexOf(event), 1);
  }

  onFeaturedChange(value: any) {
    if (value === 'false') {
      this.hotelForm.controls['featuredFrom'].disable();
      this.hotelForm.controls['featuredTo'].disable();
   } else {
      this.hotelForm.controls['featuredFrom'].enable();
      this.hotelForm.controls['featuredTo'].enable();
    }
  }

  calculateFileSize(fileSize: number) {
    const size = Math.ceil(fileSize);
    if (size < 1000) {
      this.size = size;
      this.unit = 'bytes';
    } else if (size < 1000 * 1000) {
      this.size = size / 1000;
      this.unit = 'kb';
    } else if (size < 1000 * 1000  * 1000) {
      this.size = size / 1000 / 1000;
      this.unit = 'mb';
    } else {
      this.size = size / 1000 / 1000 / 1000;
      this.unit = 'gb';
    }
    return this.size + ' ' + this.unit;
  }

  update() {
    this.formSubmitted = true;
    if (this.hotelForm.valid) {
      this.mapFormToModel();
      this.loading = true;
      this.hotelService.update(this.hotel).subscribe((event)  => {
        this.alertify.success('Hotel Updated Successfuly');
      }, error => {
        this.alertify.error(error);
        this.loading = false;
      }, () => {
        this.loading = false;
        //this.router.navigate(['/confirm', {email: this.user.email}]);
      });
    } else {
      this.alertify.error('Please fix the errors!');
    }

  }

  mapFormToModel() {
    if (this.files != null && this.files.length > 0) {
      this.hotelForm.controls['images'].setValue(this.files);
    }
    this.hotel = Object.assign({}, this.hotelForm.value);
    //console.log('isFeatured: ' + Boolean(this.hotel.isFeatured));
    if (Boolean(JSON.parse(this.hotel.isFeatured)) === true) {
      this.convertFeaturedDates();
    } else {
      this.hotel.featuredTo = '0001-01-01';
      this.hotel.featuredFrom = '0001-01-01';
    }
    console.log(this.hotel);
  }

  convertFeaturedDates() {
    this.hotel.featuredFrom = this.commonService.formatDate(this.hotel.featuredFrom, false);
    this.hotel.featuredTo = this.commonService.formatDate(this.hotel.featuredTo, false);
  }

}
