import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Cat, AdoptionApplication } from '../models/cat.model';

@Injectable({
  providedIn: 'root'
})
export class CatService {
  private cats: Cat[] = generateSampleCats();

  getCats(): Observable<Cat[]> {
    return of(this.cats);
  }

  getCatById(id: string): Observable<Cat | undefined> {
    return of(this.cats.find(cat => cat.id === id));
  }

  submitAdoptionApplication(application: AdoptionApplication): Observable<boolean> {
    console.log('Application submitted:', application);
    return of(true);
  }
}

function generateSampleCats(): Cat[] {
  const breeds = [
    'Siamese', 'Persian', 'Maine Coon', 'British Shorthair', 'Ragdoll',
    'Bengal', 'Sphynx', 'Russian Blue', 'Scottish Fold', 'American Shorthair'
  ];
  
  const names = [
    'Luna', 'Oliver', 'Bella', 'Leo', 'Lucy', 'Max', 'Lily', 'Charlie', 'Milo',
    'Sophie', 'Jack', 'Chloe', 'Rocky', 'Molly', 'Simba', 'Nala', 'Oscar', 'Ruby',
    'Felix', 'Daisy'
  ];

  const descriptions = [
    'A sweet and gentle cat who loves cuddles',
    'Playful and energetic, always ready for fun',
    'Calm and majestic, perfect for a quiet home',
    'Very social and great with other pets',
    'Loves attention and being the center of the household'
  ];

  return Array.from({ length: 100 }, (_, i) => ({
    id: (i + 1).toString(),
    name: names[Math.floor(Math.random() * names.length)],
    breed: breeds[Math.floor(Math.random() * breeds.length)],
    age: Math.floor(Math.random() * 15) + 1,
    description: descriptions[Math.floor(Math.random() * descriptions.length)],
    imageUrl: `https://placekitten.com/${400 + (i % 10)}/${300 + (i % 10)}`,
    isAdopted: Math.random() > 0.8,
    goodWithKids: Math.random() > 0.3,
    goodWithDogs: Math.random() > 0.4,
    playfulness: Math.floor(Math.random() * 5) + 1
  }));
}