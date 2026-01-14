import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // 1. Get token safely
    const token = this.authService.getToken();

    // 2. Clone request and add Bearer token if it exists
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    // 3. Handle the request and catch 401 errors
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          console.error('401 Unauthorized detected. Redirecting to login...');

          this.authService.logout(); // Ensure this clears localStorage/Session
          this.router.navigate(['/auth/login'], {
            queryParams: { returnUrl: this.router.url }
          });
        }
        return throwError(() => error);
      })
    );
  }
}
