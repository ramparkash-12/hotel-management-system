import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HotelService } from 'src/app/admin/hotel/hotel.service';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';
import { CommonService } from 'src/app/shared/services/common.service';

@Component({
  selector: 'app-book',
  templateUrl: './book.component.html',
  styleUrls: ['./book.component.css']
})
export class BookComponent implements OnInit {
  adults: number;
  dates: any;
  dateFrom: string;
  dateTo: string;
  hotelId: number;
  hotel: Hotel;
  convertDatesString = '';
  renderedHtmlDescription: any;
  bookingForm: FormGroup;
  formSubmitted: Boolean = false;
  isLoading = false;
  @ViewChild('dataContainer', null) dataContainer: ElementRef;

  constructor(private route: ActivatedRoute, private commonService: CommonService,
              private service: HotelService, private alertify: AlertifyService,
              private fb: FormBuilder) { }

  ngOnInit() {
    this.adults = this.route.snapshot.queryParams['Adults'];
    this.dateFrom = this.commonService.formatDate(this.route.snapshot.queryParams['DateFrom'], false);
    this.dateTo = this.commonService.formatDate(this.route.snapshot.queryParams['DateTo'], false);
    this.hotelId = +this.route.snapshot.params['hotelId'];

    // Load info. from server
    this.load();
    this.createBookingForm();
  }

  createBookingForm() {
    this.bookingForm = this.fb.group({
      cardHolderName: [null, [Validators.required, Validators.maxLength(100)]],
      creditCardNumber: ['', [Validators.required, Validators.minLength(16)]],
      expiryMM: ['', Validators.required],
      expiryYY: ['', Validators.required],
      cvv: ['', Validators.required]
    });
  }


  load() {
    this.service.getHotelById(this.hotelId).subscribe(res => {
      console.log(res);
      this.hotel = res;
    }, error => {
      this.alertify.error(error);
    });
  }

  onRenderedHtmlChange(data) {
    this.dataContainer.nativeElement.innerHTML = data;
    this.renderedHtmlDescription = this.dataContainer.nativeElement.innerText.substr(0, 75) + '.....';
    return this.renderedHtmlDescription;
  }

  onSave() {
    this.formSubmitted = true;
    if (this.bookingForm.valid) {
      this.hotel = Object.assign({}, this.bookingForm.value);
      this.isLoading = true;
      this.service.saveHotel(this.hotel).subscribe(() => {
        // navigate to booking confirmed page.
      //this.router.navigate(['/confirm', {email: this.user.email}]);
      }, error => {
        this.alertify.error(error);
        this.isLoading = false;
      }, () => {
        this.isLoading = false;
      });
    } else {
      this.alertify.error('Please fix the errors!');
    }
  }

}
