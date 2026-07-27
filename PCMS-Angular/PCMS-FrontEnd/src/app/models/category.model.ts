import { Product } from "./product.model";

export interface Category {
  id: number;
  name: string;
  description: string;
  parentCategoryId?: number;

  parentCategory?: Category;
  subCategories?: Category[];
  products?: Product[];         
}