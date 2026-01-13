import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class JwtInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token =
      typeof this.authService?.getToken === 'function'
        ? this.authService.getToken()
        : (this.authService as any)?.token ?? null;

    const authorizedReq = token
      ? req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        })
      : req;

    return next.handle(authorizedReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Clear authentication state and navigate to login
          try {
            if (typeof this.authService?.logout === 'function') {
              this.authService.logout();
            }
          } finally {
            this.router.navigate(['/auth/login']);
          }
        }
        return throwError(() => error);
      })
    );
  }
}
