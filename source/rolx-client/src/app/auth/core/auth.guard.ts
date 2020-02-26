import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import moment from 'moment';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private authService: AuthService,
  ) { }

  async canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    await this.authService.initialize();
    return this.isAuthorized(state);
  }

  private isAuthorized(state: RouterStateSnapshot) {
    const currentUser = this.authService.currentUser;
    if (!currentUser) {
      return this.router.createUrlTree(['/sign-in'], { queryParams: { forwardRoute: state.url } });
    }

    if (!currentUser.isActiveAt(moment())) {
      return this.router.createUrlTree(['/forbidden']);
    }

    return true;
  }
}