import { Component, OnInit } from '@angular/core';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from 'ngx-gallery';

@Component({
  selector: 'app-search-hotel-item-detail',
  templateUrl: './search-hotel-item-detail.component.html',
  styleUrls: ['./search-hotel-item-detail.component.css']
})
export class SearchHotelItemDetailComponent implements OnInit {
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];

  constructor() { }

  ngOnInit() {
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
