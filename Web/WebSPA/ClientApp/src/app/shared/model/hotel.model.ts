export interface Hotel {
  id: number;
  name: string;
  description: string;
  address: string;
  city: string;
  country: string;
  status: string;
  stars: number;
  isFeatured: string;
  featuredFrom: string;
  featuredTo: string;
  images: File [];
}