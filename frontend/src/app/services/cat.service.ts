import { Injectable } from '@angular/core';
import { Observable, catchError, of, throwError, tap } from 'rxjs';
import { Cat, AdoptionApplication } from '../models/cat.model';
import { environment } from '../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CatService {
  private readonly baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {
    console.log('CatService initialized');
  }

  private cats: Cat[] = generateSampleCats();
  // The getCats method returns an observable that emits an array of Cat objects.
  getCats(tenantId: string): Observable<Cat[]> {
    console.log(`Fetching cats for tenant: ${tenantId}`);
    return this.http.get<Cat[]>(`${this.baseUrl}/Cats?tenantId=${tenantId}`).pipe(
      tap(response => console.log('Successfully fetched cat:', response)),
      catchError((error: HttpErrorResponse) => throwError(() => new Error(error.error || 'An error occurred fetching cats'))
      )
    );
  }
  // The getCatById method returns an observable that emits a single Cat object with the specified ID.
  getCatById(id: string): Observable<Cat | undefined> {
    return this.http.get<Cat>(`${this.baseUrl}/Cats/${id}`).pipe(
      catchError((error: HttpErrorResponse) => throwError(() => new Error(error.error || 'An error occurred fetching the cat'))
      )
    );
  }

  submitAdoptionApplication(application: AdoptionApplication): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}/Adoptions`, application).pipe(
      catchError((error: HttpErrorResponse) => throwError(() => new Error(error.error || 'An error occurred submitting the application'))
      )
    );
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