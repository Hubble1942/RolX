import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/auth/core/auth.service';
import { Subproject } from '@app/projects/core/subproject';
import { map } from 'rxjs';

@Component({
  selector: 'rolx-subproject-page-layout',
  templateUrl: './subproject-page-layout.component.html',
  styleUrls: ['./subproject-page-layout.component.scss'],
})
export class SubprojectLayoutPageComponent {
  readonly mayEdit = this.authService.currentApprovalOrError.isSupervisor;
  readonly tabs: { label: string; path: string }[] = [
    { label: 'Aktivitäten', path: '/activity' },
    { label: 'Buchungen', path: '/record' },
  ];
  @Input() subproject?: Subproject;

  tabs$ = this.activeRoute.url.pipe(
    map(() =>
      this.tabs.map((tab) => {
        const path = '/subproject/' + this.subproject?.id + tab.path;
        return {
          label: tab.label,
          path,
          isCurrent: path === this.router.url,
        };
      }),
    ),
  );

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private readonly authService: AuthService,
  ) {}
}
