import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login {
  email = '';
  password = '';
  error = '';

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit(form?: NgForm) {
    if (!form?.valid) {
      this.error = 'Please fill out the form correctly.';
      return;
    }

    this.authService.login(this.email, this.password).subscribe({
      next: (user) => {
        localStorage.setItem('currentUserId', String(user.id));
        this.router.navigate(['/rent']);
      },
      error: (err) => (this.error = err.error?.message || 'Login failed')
    });
  }
}