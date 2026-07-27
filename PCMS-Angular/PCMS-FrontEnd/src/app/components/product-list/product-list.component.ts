import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.svc';
import { Product } from '../../models/product.model';
import { SearchBarComponent } from '../search-bar/search-bar.component';
import { CategoryFilterComponent } from '../category-filter/category-filter.component';
import { Category } from '../../models/category.model';
import { Router } from '@angular/router';
import { CategoryService } from '../../services/category.svc';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [
    CommonModule, FormsModule, SearchBarComponent,
    CategoryFilterComponent],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss',
})
export class ProductListComponent implements OnInit {
  //models
  products: Product[] = [];
  categories: Category[] = [];

  selectedEndpoint: string = 'default';
  //filtering and searching
  searchTerm: string = '';
  selectedCategoryId: number | null = null;
  loading = false;

  //sorting functionality
  sortBy: string = 'name';
  sortDirection: 'asc' | 'desc' = 'asc';

  //error message handling
  error: string | null = null;

  constructor(
    private productService: ProductService,
    private categoryService: CategoryService,
    private router: Router) { }

  ngOnInit(): void {
    this.categoryService.getTree().subscribe({
      next: (data) => {
        this.categories = data;
        console.log('Loaded categories:', this.categories);
      },
      error: (err) => console.error('Failed to load categories', err)
    });

    this.loadProducts();
  }

  onSearch(term: string): void {
    this.searchTerm = term;
    this.loadProducts();
  }

  onFilter(categoryId: number | null): void {
    this.selectedCategoryId = categoryId;
    this.loadProducts();
  }


  //Load list of all products (ore per filtering/searching criteria)
  loadProducts(): void {
    const params: any = {};
    if (this.searchTerm) params.name = this.searchTerm;
    if (this.selectedCategoryId) params.category = this.selectedCategoryId;

    params.sortBy = this.sortBy;
    params.sortDirection = this.sortDirection;

    this.error = null;

    let request$;
    switch (this.selectedEndpoint) {
      case 'customJson':
        request$ = this.productService.getCustomJson(params);
        break;
      case 'manualLookup':
        request$ = this.productService.manualLookup(params);
        break;
      default:
        request$ = this.productService.getAll(params);
        break;
    }

    request$.subscribe({
      next: (data) => {
        this.products = data;
      },
      error: (err) => {
        this.error = 'Failed to load products';
        console.error(err);
      }
    });
  }

  goToCreate(): void {
    this.router.navigate(['/products/add']);
  }

  editProduct(id: number): void {
    this.router.navigate(['/products/edit', id]);
  }

  confirmDelete(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.delete(id).subscribe(() => this.loadProducts());
    }
  }

  toggleSort(field: string): void {
    if (this.sortBy === field) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortBy = field;
      this.sortDirection = 'asc';
    }
    this.loadProducts();
  }
}
