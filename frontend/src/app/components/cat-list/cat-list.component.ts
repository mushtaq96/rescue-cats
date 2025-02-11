import { Component, OnInit } from '@angular/core';
import { LeafletModule } from '@bluehalo/ngx-leaflet';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CatService } from '../../services/cat.service';
import { Cat } from '../../models/cat.model';
import * as L from 'leaflet';

@Component({
  selector: 'app-cat-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, LeafletModule],
  templateUrl: './cat-list.html'
})
export class CatListComponent implements OnInit {
  // Map setup
  map!: L.Map; // instance of the Leaflet map
  mapError = ''; // error message for map initialization
  mapOptions: L.MapOptions = {
    layers: [
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
      })
    ],
    zoom: 2,
    center: L.latLng(20, 0)
  };

  allCats: Cat[] = [];
  filteredCats: Cat[] = [];
  uniqueBreeds: string[] = [];
  displayMode: 'list' | 'map' = 'list';
  // Search and filter
  filters = {
    name: '',
    breed: '',
    minAge: null as number | null,
    maxAge: null as number | null
  };

  constructor(private catService: CatService) { }

  ngOnInit() {
    this.loadCats();
  }
  private loadCats() {
    const tenantId = 'tenantA';
    this.catService.getCats(tenantId).subscribe(cats => {
      this.allCats = cats;
      this.filteredCats = cats;
      this.uniqueBreeds = [...new Set(cats.map(cat => cat.breed))].sort();
      console.log('CatListComponent initialized');
      console.log('allCats:', this.allCats);
      console.log('uniqueBreeds:', this.uniqueBreeds);
    });
  }
  // apply filters when user inputs in search bar or selects options in dropdowns
  applyFilters() {
    this.filteredCats = this.allCats.filter(cat => {
      const nameMatch = !this.filters.name || cat.name.toLowerCase().includes(this.filters.name.toLowerCase());
      const breedMatch = !this.filters.breed || cat.breed === this.filters.breed;
      const minAgeMatch = !this.filters.minAge || cat.age >= this.filters.minAge;
      const maxAgeMatch = !this.filters.maxAge || cat.age <= this.filters.maxAge;
      return nameMatch && breedMatch && minAgeMatch && maxAgeMatch;
    });
  }
  // reset filters to default state
  resetFilters() {
    this.filters = {
      name: '',
      breed: '',
      minAge: null,
      maxAge: null
    };
    this.filteredCats = this.allCats;
  }
  // toggle between list and map view
  toggleDisplayMode(mode: 'list' | 'map') {
    this.displayMode = mode;
  }
  // Map functionality
  onMapReady(map: L.Map) {
    this.map = map;
    this.updateMapMarkers();
  }
  // Update map markers when cat data changes
  updateMapMarkers() {
    if (this.map) {
      this.filteredCats.forEach(cat => {
        const marker = L.marker([cat.location.latitude, cat.location.longitude], {
          title: cat.name,
        }).bindPopup(`
          <div class="cat-popup">
            <img src="${cat.imageUrl}" alt="${cat.name}" style="max-width: 150px; max-height: 150px;"
            onclick="window.location.href='/cats/${cat.id}'">
            <b>${cat.name}</b><br>
            ${cat.breed}<br>
            ${cat.location.city}
           </div>
        `);

        marker.addTo(this.map);
      });
    }
  }
}

const styles = `
.cat-popup {
    max-width: 200px;
    padding: 05px;
}
.cat-popup img {
    margin-bottom: 10px;
    border-radius: 8px;
}
`;
const styleSheet = document.createElement("style");
styleSheet.innerText = styles;
document.head.appendChild(styleSheet);