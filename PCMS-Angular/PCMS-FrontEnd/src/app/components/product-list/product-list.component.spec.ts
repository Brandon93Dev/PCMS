import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { of } from 'rxjs';
import { ProductService } from '../../services/product.svc';
import { ProductListComponent } from './product-list.component';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let productService: jasmine.SpyObj<ProductService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    productService = jasmine.createSpyObj('ProductService', ['getAll', 'getCustomJson', 'manualLookup', 'delete']);
    productService.getAll.and.returnValue(of([{ id: 1, name: 'Test Product', description: '', sku: 'SKU1', price: 10, quantity: 2, categoryId: 1, createdAt: '', modifiedAt: '' }]));
    router = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [ProductListComponent],
      providers: [
        { provide: ProductService, useValue: productService },
        { provide: Router, useValue: router }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load products on initialization', () => {
    fixture.detectChanges();

    expect(productService.getAll).toHaveBeenCalled();
    expect(component.products.length).toBe(1);
    expect(component.loading).toBeFalse();
  });
});
