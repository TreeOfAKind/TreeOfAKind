import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { switchMap, take } from 'rxjs/operators';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(
    private authService: AuthService
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const user = this.authService.getUser();

    if (user == null) {
      return next.handle(req);
    }


    return this.authService.getToken().pipe(
      take(1),
      switchMap(token => {
        req = req.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`
          }
        })
        return next.handle(req);
      })
    );
  }

}
