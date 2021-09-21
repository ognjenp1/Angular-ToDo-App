import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, pipe } from 'rxjs';
import { AuthService } from '@auth0/auth0-angular';
import { mergeMap } from 'rxjs/operators';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  token!: string;
  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler) {
    // Get the auth token from the service.
    return this.auth.getAccessTokenSilently().pipe(
      mergeMap((token) => {
        console.log(token);
        // Clone the request and replace the original headers with
        // cloned headers, updated with the authorization.
        const authReq = req.clone({
          headers: req.headers.set(`Authorization`, `Bearer ${token}`),
        });

        return next.handle(authReq);
      })
    );
  }
}
