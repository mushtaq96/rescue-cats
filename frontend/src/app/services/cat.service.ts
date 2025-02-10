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
      tap(response => console.log('Successfully fetched cat:', response)),
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
