export interface Hotel {
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  status: boolean;
  stars: number;
  isFeatured: boolean;
  featuredFrom: Date;
  featureTo: Date;
  images: File [];
}