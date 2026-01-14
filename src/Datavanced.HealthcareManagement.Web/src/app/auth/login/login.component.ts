import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  submitted = false;
  errorMessage = '';
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }


  get f() {
    return this.loginForm.controls;
  }

  private initializeForm(): void {
    this.loginForm = this.fb.group({
      username: [
        '',
        { validators: [Validators.required, Validators.minLength(3)], nonNullable: true }
      ],
      password: [
        '',
        { validators: [Validators.required, Validators.minLength(6)], nonNullable: true }
      ]
    });
  }

  onSubmit(): void {
    this.submitted = true;
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      return;
    }

    this.isLoading = true;
    const credentials = this.loginForm.getRawValue();
    this.authService.logout();

    this.authService.login(credentials).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/patient']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Invalid username or password';
        console.error('Login error:', err);
      }
    });
  }
}
