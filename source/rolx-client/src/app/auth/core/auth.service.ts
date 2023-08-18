import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '@env/environment';
import { instanceToPlain, plainToInstance } from 'class-transformer';
import { interval, lastValueFrom, switchMap } from 'rxjs';
import { filter } from 'rxjs/operators';

import { Approval } from './approval';
import { SignInService } from './sign-in.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private static readonly CurrentApprovalKey = 'currentApproval';

  private _currentApproval?: Approval;
  private isInitialized = false;

  get currentApproval() {
    return this._currentApproval;
  }

  get currentApprovalOrError() {
    if (this._currentApproval == null) {
      throw new Error('Not logged in properly');
    }

    return this._currentApproval;
  }

  constructor(private signInService: SignInService, private router: Router) {
    console.log('--- AuthService.ctor()');

    interval(5 * 60 * 1000)
      .pipe(
        filter(() => this.currentApproval != null),
        switchMap(() => this.signInService.extend()),
      )
      .subscribe((approval) => {
        if (approval) {
          console.log('--- AuthService: backend access extended');
          this.setCurrentApproval(approval);
        } else if (environment.production) {
          console.log('--- AuthService: backend access extension denied');
          this.signOut();

          // noinspection JSIgnoredPromiseFromCall
          this.router.navigate(['/sign-in']);
        }
      });
  }

  async initialize(): Promise<void> {
    if (this.isInitialized) {
      return;
    }

    console.log('--- AuthService.initialize()');
    this.isInitialized = true;

    const approval = AuthService.LoadCurrentApproval();
    if (!approval || approval.isExpired) {
      this.signOut();
      console.log('--- AuthService.initialize() done');
      return;
    }

    this.setCurrentApproval(approval);

    console.log('--- AuthService.initialize() done');
  }

  async signIn(googleIdToken: string): Promise<void> {
    const approval = await lastValueFrom(
      this.signInService.signIn({
        googleIdToken,
      }),
    );

    this.setCurrentApproval(approval);
  }

  signOut() {
    AuthService.ClearCurrentApproval();
    this._currentApproval = undefined;
  }

  private static LoadCurrentApproval(): Approval | null {
    const approvalJson = localStorage.getItem(AuthService.CurrentApprovalKey);
    if (!approvalJson) {
      return null;
    }

    const approvalPlain = JSON.parse(approvalJson);
    const approval = plainToInstance(Approval, approvalPlain);

    try {
      approval.validateModel();
    } catch (e) {
      console.warn('failed to restore approval', e);
      return null;
    }

    return approval;
  }

  private static StoreCurrentApproval(approval: Approval) {
    localStorage.setItem(AuthService.CurrentApprovalKey, JSON.stringify(instanceToPlain(approval)));
  }

  private static ClearCurrentApproval() {
    localStorage.removeItem(AuthService.CurrentApprovalKey);
  }

  private setCurrentApproval(approval: Approval) {
    AuthService.StoreCurrentApproval(approval);
    this._currentApproval = approval;
  }
}
