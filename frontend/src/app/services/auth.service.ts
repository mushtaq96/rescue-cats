import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, Observable, of, throwError, tap } from 'rxjs';
import { User, LoginCredentials, SignupCredentials } from '../models/user.model';
import { environment } from '../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
// The AuthService class is responsible for handling user authentication. It provides methods for logging in, signing up, and logging out.
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl = environment.baseUrl;
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient, private router: Router) {
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
  }

  // The login method sends a POST request to the /User/login endpoint with the provided credentials.
  login(credentials: LoginCredentials): Observable<{ token: string, user: User }> {
    return this.http.post<{
      token: string, user: User
    }>(`${this.baseUrl}/User/login`, credentials).pipe(
      tap((response) => {
        localStorage.setItem('currentUser', JSON.stringify(response.user));

        this.currentUserSubject.next(response.user);
        console.log('Successfully logged in:', response);
        // redirect to the cats page
        this.router.navigate(['/cats']);
      }),

      catchError((error: HttpErrorResponse) => throwError(() => new Error(error.error || 'An error occurred Login failed')))
    );
  }

  signup(credentials: SignupCredentials): Observable<User> {
    const user: Partial<User> = {
      email: credentials.email,
      details: { firstName: '', lastName: '' }, // Initialize empty details
      location: { street: '', city: '', state: '', postalCode: '', latitude: 0, longitude: 0 }, // Initialize empty location
      password: credentials.password,
      role: 'user'
    };
    return this.http.post<User>(`${this.baseUrl}/User/register`, user).pipe(
      catchError((error: HttpErrorResponse) => throwError(() => new Error(error.error || 'An error occurred Signup failed'))
      )
    );
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  isAuthenticated(): boolean {
    return this.currentUserSubject.value !== null;
  }
}