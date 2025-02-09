import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html'
})
export class LoginComponent {
  credentials = {
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
    this.authService.login(this.credentials).subscribe({
      next: () => this.router.navigate(['/cats']),
      error: (err) => this.error = err.message
    });
  }
}