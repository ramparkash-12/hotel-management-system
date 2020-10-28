import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';
import { HotelService } from 'src/app/admin/hotel/hotel.service';
import { Hotel } from 'src/app/shared/model/hotel.model';
import { AlertifyService } from 'src/app/shared/services/alertify.service';

@Component({
  selector: 'app-search-hotel-item-detail',
  templateUrl: './search-hotel-item-detail.component.html',
  styleUrls: ['./search-hotel-item-detail.component.css']
})
export class SearchHotelItemDetailComponent implements OnInit {
  paramId: any;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  hotel: Hotel;
  searchCriteria: any = {
    city: '',
    adults: 1
   } ;
   bookParams: any = { };

  constructor(private route: ActivatedRoute, private service: HotelService,
              private alertify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.paramId = this.route.snapshot.params['hotelId'];

    this.load();

    this.galleryOptions = [
      {
          width: '800px',
          height: '600px',
          thumbnailsColumns: 4,
          imageAnimation: NgxGalleryAnimation.Slide
      },
      // max-width 800
      {
          breakpoint: 800,
          width: '100%',
          height: '600px',
          imagePercent: 80,
          thumbnailsPercent: 20,
          thumbnailsMargin: 20,
          thumbnailMargin: 20
      },
      // max-width 400
      {
          breakpoint: 400,
          preview: false
      }
    ];

    this.galleryImages = [];

  }

  load() {
    this.service.getHotelById(this.paramId).subscribe(res => {
      console.log(res);
      this.hotel = res;
      this.setImages(res);
    }, error => {
      this.alertify.error(error);
    });
  }

  onBooking() {
    this.router.navigate(['book']);
  }


  private setImages(model: any) {
    if (model !== null && model.images.length > 0) {
      model.images.forEach(item => {
        this.galleryImages.push({
          small: item.uri,
          medium: item.uri,
          big: item.uri
        });
      });
    } else {
      this.galleryImages = [
        {
          small: '../../../../assets/images/hotel_image.png',
          medium: '../../../../assets/images/hotel_image.png',
          big: '../../../../assets/images/hotel_image.png'
        },
        {
          small: '../../../../assets/images/hotel_image.png',
          medium: '../../../../assets/images/hotel_image.png',
          big: '../../../../assets/images/hotel_image.png'
        },
        {
          small: '../../../../assets/images/hotel_image.png',
          medium: '../../../../assets/images/hotel_image.png',
          big: '../../../../assets/images/hotel_image.png'
        },
        {
          small: '../../../../assets/images/hotel_image.png',
          medium: '../../../../assets/images/hotel_image.png',
          big: '../../../../assets/images/hotel_image.png'
        }
      ];
    }
  }

}
