import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
export class HttpErrorInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          let errorMsg: string;
          if (error.error instanceof ErrorEvent) {
            errorMsg = `Error: ${error.error.message}`;
          }
          else {
            errorMsg = `Error: ${error.error.detail}`;
          }
          window.alert(errorMsg);
          return throwError(errorMsg);
        })
      )
  }
}
