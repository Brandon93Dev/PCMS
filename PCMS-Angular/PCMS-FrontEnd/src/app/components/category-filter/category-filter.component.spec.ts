import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CategoryFilterComponent } from './category-filter.component';

describe('CategoryFilterComponent', () => {
  let component: CategoryFilterComponent;
  let fixture: ComponentFixture<CategoryFilterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CategoryFilterComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(CategoryFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should emit the selected category id when filtering', () => {
    component.selectedCategoryId = 2;
    const emitSpy = spyOn(component.filter, 'emit');

    component.onFilter();

    expect(emitSpy).toHaveBeenCalledWith(2);
  });
});
