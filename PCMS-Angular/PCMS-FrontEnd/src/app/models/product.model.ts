export interface Product {
  id: number;
  name: string; 
  description: string;
  sku: string;
  price: number; 
  quantity: number; 
  categoryId: number;
  createdAt: string;
  modifiedAt: string;
}