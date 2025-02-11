import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SignupCredentials } from '../../models/user.model';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './signup.html'
})
export class SignupComponent {
  credentials = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    street: '',
    city: '',
    state: '',
    postalCode: '',
    latitude: 0,
    longitude: 0
  };
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit() {
    this.error = '';
    if (!this.credentials.latitude || !this.credentials.longitude) {
      this.error = 'Please allow location access or manually enter your address.';
      return;
    }

    const signupCredentials: SignupCredentials = {
      firstName: this.credentials.firstName,
      lastName: this.credentials.lastName,
      email: this.credentials.email,
      password: this.credentials.password
    };

    this.authService.signup(signupCredentials).subscribe({
      next: () => this.router.navigate(['/cats']),
      error: (err) => this.error = err.message
    });
  }

  getCurrentLocation() {
    console.log('Getting current location...');
    console.log('navigator:', navigator);
    if ('geolocation' in navigator) {
      navigator.geolocation.getCurrentPosition((position) => {
        this.credentials.latitude = position.coords.latitude;
        this.credentials.longitude = position.coords.longitude;
        console.log('Current location:', this.credentials.latitude, this.credentials.longitude);

        // Optionally call a reverse geocoding service here to fill in street, city, etc.
      }, (error) => {
        switch (error.code) {
          case error.PERMISSION_DENIED:
            this.error = 'User denied the request for Geolocation.';
            break;
          case error.POSITION_UNAVAILABLE:
            this.error = 'Location information is unavailable.';
            break;
          case error.TIMEOUT:
            this.error = 'The request to get user location timed out.';
            break;
          default:
            this.error = 'An unknown error occurred.';
        }
      });
    } else {
      this.error = 'Geolocation is not supported by this browser.';
    }
  }
}