import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { routes } from './app/app.routes';
import { AuthService } from './app/services/auth.service';
import { provideHttpClient } from '@angular/common/http';

// Angular version 18
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  template: `
    <header class="bg-primary text-white p-4">
      <div class="container mx-auto flex justify-between items-center">
        <h1 class="text-2xl font-bold">Cat Rescue Portal</h1>
        @if (authService.currentUser$ | async; as user) {
          <div class="flex items-center gap-4">
            <span>Welcome, {{user.details.firstName}}</span>
            <button 
              (click)="logout()"
              class="bg-white text-primary px-4 py-2 rounded-md hover:bg-gray-100"
            >
              Logout
            </button>
          </div>
        } @else {
          <div class="space-x-4">
            <a 
              routerLink="/login"
              class="bg-white text-primary px-4 py-2 rounded-md hover:bg-gray-100"
            >
              Login
            </a>
            <a 
              routerLink="/signup"
              class="bg-white text-primary px-4 py-2 rounded-md hover:bg-gray-100"
            >
              Sign up
            </a>
          </div>
        }
      </div>
    </header>
    <main>
      <router-outlet></router-outlet> 
    </main>
  `
})
export class App {
  constructor(public authService: AuthService) { }

  logout() {
    this.authService.logout();
  }
}

bootstrapApplication(App, {
  providers: [
    provideRouter(routes),
    provideHttpClient()
  ]
});