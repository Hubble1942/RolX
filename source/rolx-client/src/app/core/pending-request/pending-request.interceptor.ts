import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError, finalize } from 'rxjs/operators';

import { PendingRequestService } from './pending-request.service';

@Injectable()
export class PendingRequestInterceptor implements HttpInterceptor {
  public static NO_REDIRECT_ON_ERROR_HEADER = 'X-ROLX-NO-REDIRECT-ON-ERROR';
  public static NO_REQUEST_TRACKING_HEADER = 'X-ROLX-NO-REQUEST-TRACKING';

  constructor(private pendingRequestService: PendingRequestService, private router: Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (request.headers.has(PendingRequestInterceptor.NO_REQUEST_TRACKING_HEADER)) {
      request = request.clone({
        headers: request.headers.delete(PendingRequestInterceptor.NO_REQUEST_TRACKING_HEADER),
      });

      return next.handle(request);
    }

    this.pendingRequestService.requestStarted();

    const redirectOnError = !request.headers.has(
      PendingRequestInterceptor.NO_REDIRECT_ON_ERROR_HEADER,
    );

    if (!redirectOnError) {
      request = request.clone({
        headers: request.headers.delete(PendingRequestInterceptor.NO_REDIRECT_ON_ERROR_HEADER),
      });
    }

    let interceptedNext = next
      .handle(request)
      .pipe(finalize(() => this.pendingRequestService.requestFinished()));

    if (redirectOnError && request.method === 'GET') {
      interceptedNext = interceptedNext.pipe(catchError((err) => this.handleServerError(err)));
    }

    return interceptedNext;
  }

  private handleServerError(err: any) {
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['/server-error']);

    return throwError(() => err);
  }
}
