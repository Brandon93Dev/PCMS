import { Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductFormComponent } from './components/product-form/product-form.component';
import { CategoryFilterComponent } from './components/category-filter/category-filter.component';

export const routes: Routes = [
    { path: 'products', component: ProductListComponent },
    { path: 'products/add', component: ProductFormComponent },
    { path: 'products/edit/:id', component: ProductFormComponent },
    { path: 'categories', component: CategoryFilterComponent },
    { path: '', redirectTo: '/products', pathMatch: 'full' }
];
