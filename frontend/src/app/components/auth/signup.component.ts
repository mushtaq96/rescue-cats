import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './signup.html'
})
export class SignupComponent {
  credentials = {
    name: '',
    email: '',
    password: ''
  };
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit() {
    this.error = '';
    this.authService.signup(this.credentials).subscribe({
      next: () => this.router.navigate(['/cats']),
      error: (err) => this.error = err.message
    });
  }
}