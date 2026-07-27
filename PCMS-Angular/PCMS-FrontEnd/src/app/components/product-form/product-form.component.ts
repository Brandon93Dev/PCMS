import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../../services/product.svc';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoryService } from '../../services/category.svc';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss',
})
export class ProductFormComponent implements OnInit {
  productForm!: FormGroup;
  isEdit = false;
  productId!: number;
  categories: Category[] = [];
  error: string | null = null;

  constructor(
    private formbuilderb: FormBuilder,
    private productService: ProductService,
    private categoryService: CategoryService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.productForm = this.formbuilderb.group({
      name: ['', Validators.required],
      description: [''],
      sku: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      quantity: [0, [Validators.required, Validators.min(0)]],
      categoryId: [null, Validators.required]
    });

     this.categoryService.getTree().subscribe({
    next: (data) => {
      this.categories = data;
      console.log('Loaded categories:', this.categories);
    },
    error: (err) => console.error('Failed to load categories', err)
  });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEdit = true;
        this.productId = +id;
        this.productService.getById(this.productId).subscribe(product => {
          this.productForm.patchValue(product);
        });
      }
    });
  }

onSubmit(): void {
  if (this.productForm.invalid) return;

  const action$ = this.isEdit
    ? this.productService.update(this.productId, this.productForm.value)
    : this.productService.add(this.productForm.value);

  action$.subscribe({
    next: () => this.router.navigate(['/products']),
    error: (err : any) => {
      this.error = err.error || 'Duplicate product not allowed';
      console.error(err);
    }
  });
}
}
