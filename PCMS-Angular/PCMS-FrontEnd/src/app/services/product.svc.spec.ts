import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { ProductService } from './product.svc';

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ProductService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should fetch products', () => {
    const dummyProducts = [{ id: 1, name: 'Test', description: '', sku: 'ABC', price: 10, quantity: 5, categoryId: 1, createdAt: '', modifiedAt: '' }];

    service.getAll().subscribe(products => {
      expect(products.length).toBe(1);
      expect(products[0].name).toBe('Test');
    });

    const req = httpMock.expectOne('https://localhost:7017/api/products');
    expect(req.request.method).toBe('GET');
    req.flush(dummyProducts);
  });

  it('should create a new product through POST', () => {
    const newProduct = { id: 2, name: 'New Product', description: '', sku: 'ABC-2', price: 20, quantity: 4, categoryId: 1, createdAt: '', modifiedAt: '' };

    service.add(newProduct).subscribe(product => {
      expect(product).toEqual(newProduct);
    });

    const req = httpMock.expectOne('https://localhost:7017/api/products');
    expect(req.request.method).toBe('POST');
    req.flush(newProduct);
  });

  afterEach(() => {
    httpMock.verify();
  });
});
