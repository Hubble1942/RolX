import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Activity } from '@app/projects/core/activity';
import { Subproject } from '@app/projects/core/subproject';
import { SubprojectService } from '@app/projects/core/subproject.service';
import { Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

@Component({
  selector: 'rolx-subproject-detail-page',
  templateUrl: './subproject-detail-page.component.html',
  styleUrls: ['./subproject-detail-page.component.scss'],
})
export class SubprojectDetailPageComponent {
  readonly subproject$ = this.route.paramMap.pipe(
    switchMap((params) => this.initializeSubproject(params.get('id'))),
    catchError((e) => {
      if (e.status === 404) {
        // noinspection JSIgnoredPromiseFromCall
        this.router.navigate(['/four-oh-four']);
      }

      return throwError(() => e);
    }),
  );

  editedActivity?: Activity;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subprojectService: SubprojectService,
  ) {}

  private initializeSubproject(idText: string | null): Observable<Subproject> {
    return this.subprojectService.getById(Number(idText));
  }
}