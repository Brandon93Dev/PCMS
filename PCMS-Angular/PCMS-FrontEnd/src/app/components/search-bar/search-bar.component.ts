import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-search-bar',
  standalone: true,
    imports: [CommonModule, FormsModule],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.scss',
})
export class SearchBarComponent {
 searchTerm: string = '';
@Output() search = new EventEmitter<string>();

onSearch(): void {
  this.search.emit(this.searchTerm);
}
}
