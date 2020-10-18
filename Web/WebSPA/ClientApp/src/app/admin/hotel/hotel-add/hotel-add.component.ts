import { DatePipe } from '@angular/common';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { stat } from 'fs';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { HotelService } from '../hotel.service';

@Component({
  selector: 'app-hotel-add',
  templateUrl: './hotel-add.component.html',
  styleUrls: ['./hotel-add.component.css']
})
export class HotelAddComponent implements OnInit {
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
    private hotelService: HotelService, public datepipe: DatePipe) { }

  ngOnInit() {
    this.createHotelForm();
  }

  createHotelForm() {
    this.hotelForm = this.fb.group({
      name: [null, [Validators.required, Validators.maxLength(100)]],
      description: ['', Validators.required],
      address: ['', Validators.required],
      country: ['', Validators.required],
      city: ['', Validators.required],
      status: ['true'],
      stars: ['1'],
      isFeatured: ['true', Validators.required],
      featuredFrom: [''],
      featuredTo: [''],
      images: ['']
    });
  }

  onSetDummyValues() {
    this.setHotelFormDummyValues();
  }

  setHotelFormDummyValues() {
    this.hotelForm.setValue({
      name: ['Travel Lodge'],
      description: ['Demo Description goes <b> here </b>.......'],
      address: ['14 grayss inn road'],
      country: ['United States'],
      city: ['California'],
      status: 'false',
      stars: ['4'],
      isFeatured: 'true',
      featuredFrom: ['07/10/2020'],
      featuredTo: ['30/10/2020'],
      images: [null]
    });
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

  save() {
    this.formSubmitted = true;
    if (this.hotelForm.valid) {
      this.mapFormToModel();
      this.loading = true;
      this.hotelService.saveHotel(this.hotel).subscribe((event: HttpEvent<any>)  => {
        switch (event.type) {
          case HttpEventType.UploadProgress:
            this.progress = Math.round((event.loaded / event.total * 100) / this.files.length);
            console.log(`Uploaded! ${this.progress}%`);
            break;
          case HttpEventType.Response:
            this.alertify.success('Hotel Added Successfuly');
            setTimeout(() => {
              this.progress = 0;
            }, 1500);
      }}, error => {
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
    if (this.hotel.isFeatured === 'true') {
      this.convertFeaturedDates();
    }
    console.log(this.hotel);
  }

  convertFeaturedDates() {
    this.hotel.featuredFrom = this.datepipe.transform(this.hotel.featuredFrom, 'yyyy-MM-dd');
    this.hotel.featuredTo = this.datepipe.transform(this.hotel.featuredTo, 'yyyy-MM-dd');
  }

}
