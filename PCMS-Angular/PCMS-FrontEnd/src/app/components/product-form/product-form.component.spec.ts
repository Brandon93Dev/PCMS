import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, convertToParamMap, Router } from '@angular/router';
import { of } from 'rxjs';
import { ProductService } from '../../services/product.svc';
import { ProductFormComponent } from './product-form.component';

describe('ProductFormComponent', () => {
  let component: ProductFormComponent;
  let fixture: ComponentFixture<ProductFormComponent>;
  let productService: jasmine.SpyObj<ProductService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    productService = jasmine.createSpyObj('ProductService', ['getById', 'add', 'update']);
    productService.getById.and.returnValue(of({ id: 1, name: 'Existing', description: '', sku: 'SKU1', price: 10, quantity: 3, categoryId: 1, createdAt: '', modifiedAt: '' }));
    productService.add.and.returnValue(of({ id: 2, name: 'New', description: '', sku: 'SKU2', price: 20, quantity: 4, categoryId: 1, createdAt: '', modifiedAt: '' }));
    router = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, ProductFormComponent],
      providers: [
        { provide: ProductService, useValue: productService },
        { provide: Router, useValue: router },
        { provide: ActivatedRoute, useValue: { paramMap: of(convertToParamMap({})) } }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(ProductFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have a required name control', () => {
    const nameControl = component.productForm.get('name');
    nameControl?.setValue('');
    expect(nameControl?.valid).toBeFalse();
  });

  it('should add a new product on submit when creating', () => {
    component.productForm.setValue({
      name: 'New Product',
      description: 'A sample item',
      sku: 'SKU-1',
      price: 15,
      quantity: 2,
      categoryId: 1
    });

    component.onSubmit();

    expect(productService.add).toHaveBeenCalled();
    expect(router.navigate).toHaveBeenCalledWith(['/products']);
  });
});