import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

  constructor(private fb: FormBuilder, private alertify: AlertifyService,
    private hotelService: HotelService) { }

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
      status: ['1'],
      stars: ['1'],
      featured: ['1', Validators.required],
      featuredFrom: [''],
      featuredTo: [''],
      images: ['']
    });
  }

  onSelect(event) {
    this.files.push(...event.addedFiles);
  }

  onRemove(event) {
    this.files.splice(this.files.indexOf(event), 1);
  }

  onFeaturedChange(value) {
    if (value !== '1') {
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
      if (this.files != null && this.files.length > 0) {
        this.hotelForm.controls['images'].setValue(this.files);
      }
      this.hotel = Object.assign({}, this.hotelForm.value);
      this.hotelService.saveHotel(this.hotel).subscribe(() => {
        this.alertify.success('Hotel Added Successfuly');
        //this.router.navigate(['/client']);
      }, error => {
        this.alertify.error(error);
      });
      //console.log(this.hotel);
    } else {
      this.alertify.error('Please fix the errors!');
    }

  }

}
