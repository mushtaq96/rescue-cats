import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CatService } from '../../services/cat.service';
import { Cat } from '../../models/cat.model';

@Component({
  selector: 'app-cat-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cat-list.html' 
})
export class CatListComponent implements OnInit {
  allCats: Cat[] = [];
  filteredCats: Cat[] = [];
  uniqueBreeds: string[] = [];

  filters = {
    name: '',
    breed: '',
    minAge: null as number | null,
    maxAge: null as number | null
  };

  constructor(private catService: CatService) { }

  ngOnInit() {
    this.catService.getCats().subscribe(cats => {
      this.allCats = cats;
      this.filteredCats = cats;
      this.uniqueBreeds = [...new Set(cats.map(cat => cat.breed))].sort();
    });
  }

  applyFilters() {
    this.filteredCats = this.allCats.filter(cat => {
      // Name filter
      const nameMatch = !this.filters.name ||
        cat.name.toLowerCase().includes(this.filters.name.toLowerCase());

      // Breed filter
      const breedMatch = !this.filters.breed ||
        cat.breed === this.filters.breed;

      // Age filter
      const minAgeMatch = !this.filters.minAge ||
        cat.age >= this.filters.minAge;
      const maxAgeMatch = !this.filters.maxAge ||
        cat.age <= this.filters.maxAge;

      return nameMatch && breedMatch && minAgeMatch && maxAgeMatch;
    });
  }

  resetFilters() {
    this.filters = {
      name: '',
      breed: '',
      minAge: null,
      maxAge: null
    };
    this.filteredCats = this.allCats;
  }
}