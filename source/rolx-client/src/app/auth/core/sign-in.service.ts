import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PendingRequestInterceptor } from '@app/core/pending-request/pending-request.interceptor';
import { mapPlainToInstance } from '@app/core/util/operators';
import { lastValueFrom, Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { Approval } from './approval';
import { Info } from './info';
import { SignInData } from './sign-in.data';

const SignInUrl = '/api/v1/signin';

@Injectable({
  providedIn: 'root',
})
export class SignInService {
  constructor(private httpClient: HttpClient) {
    console.log('--- SignInService.ctor()');
  }

  getInfo(): Observable<Info> {
    return this.httpClient.get<Info>(SignInUrl + '/info');
  }

  signIn(signInData: SignInData): Observable<Approval> {
    return this.httpClient.post(SignInUrl, signInData).pipe(
      mapPlainToInstance(Approval),
      tap((a) => a.validateModel()),
    );
  }

  async extend(): Promise<Approval | null> {
    return await lastValueFrom(
      this.httpClient
        .get(SignInUrl + '/extend', {
          headers: {
            [PendingRequestInterceptor.NO_REQUEST_TRACKING_HEADER]: 'foo',
          },
        })
        .pipe(
          mapPlainToInstance(Approval),
          tap((a) => a.validateModel()),
          catchError(() => of(null)),
        ),
    );
  }
}
