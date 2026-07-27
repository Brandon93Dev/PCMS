import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Category } from '../../models/category.model';
import { Input, Output, EventEmitter } from '@angular/core';
import { CategoryService } from '../../services/category.svc';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-category-filter',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './category-filter.component.html',
  styleUrl: './category-filter.component.scss',
})
export class CategoryFilterComponent implements OnInit {
  @Input() categories: Category[] = [];
  @Output() filter = new EventEmitter<number | null>();

  selectedCategoryId: number | null = null;
  newCategory: Category = { id: 0, name: '', description: '', parentCategoryId: -99 };
  errorMessage: string | null = null;

  constructor(private categoryService: CategoryService) { }

  ngOnInit() {
    this.loadCategoriesTree();
  }

  createCategory(categoryForm: NgForm) {
    this.categoryService.add(this.newCategory).subscribe({
      next: () => {
        this.errorMessage = null;
        this.loadCategoriesTree();

        categoryForm.resetForm({
          parentCategoryId: -99
        });
      },
      error: (err) => {
        if (err.status === 409) {
          this.errorMessage = 'Category already exists under this parent.';
        } else {
          console.error('Failed to create category', err);
        }
      }
    });
  }

  // load full hierarchial category tree
  loadCategoriesTree() {
    this.categoryService.getTree().subscribe({
      next: (data) => {
        this.categories = [...data];
        console.log('Reloaded categories:', this.categories);
      },
      error: (err) => console.error('Failed to load categories', err)
    });
  }

  onFilter() {
    this.filter.emit(this.selectedCategoryId);
  }
}
