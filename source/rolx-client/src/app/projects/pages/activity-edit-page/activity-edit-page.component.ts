import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorResponse } from '@app/core/error/error-response';
import { Activity } from '@app/projects/core/activity';
import { Subproject } from '@app/projects/core/subproject';
import { SubprojectService } from '@app/projects/core/subproject.service';
import { Observable, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

@Component({
  selector: 'rolx-activity-edit-page',
  templateUrl: './activity-edit-page.component.html',
  styleUrls: ['./activity-edit-page.component.scss'],
})
export class ActivityEditPageComponent {
  readonly subproject$: Observable<Subproject> = this.route.paramMap.pipe(
    switchMap((params) => this.initializeSubproject(params.get('id'), params.get('activityId'))),
    catchError((e) => {
      if (e.status === 404) {
        // noinspection JSIgnoredPromiseFromCall
        this.router.navigate(['/four-oh-four']);
      }

      return throwError(() => e);
    }),
  );

  activity = new Activity();
  error?: ErrorResponse;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private subprojectService: SubprojectService,
    private snackBar: MatSnackBar,
  ) {}

  cancel(subproject: Subproject) {
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['/subproject', subproject.id]);
  }

  save(subproject: Subproject, event: {activity: Activity; addAnother: boolean}) {
    this.error = undefined;
    this.subprojectService.update(subproject).subscribe({
      next: () => {
        this.snackBar.open('Aktivität erfolgreich gespeichert', undefined, {
          duration: 1500,
        });
        if (event.addAnother) {
          // If we were editing an existing activity, adjust url to represent the addition of a new one
          // noinspection JSIgnoredPromiseFromCall
          this.router.navigate(['/subproject', subproject.id, 'activity', 'new']);
          this.activity = this.newPrefilledActivity(subproject, event.activity);
        } else {
          this.cancel(subproject);
        }
      },
      error: (err) => this.error = err,
    });
  }

  private initializeSubproject(
    subprojectIdText: string | null,
    activityIdText: string | null,
  ): Observable<Subproject> {
    return this.subprojectService
      .getById(Number(subprojectIdText))
      .pipe(map((subproject) => this.initializeActivity(subproject, activityIdText)));
  }

  private initializeActivity(subproject: Subproject, activityIdText: string | null): Subproject {
    const activityId = Number(activityIdText);

    const activity =
      activityIdText === 'new'
        ? this.newPrefilledActivity(subproject, this.activity)
        : subproject.activities.find((a) => a.id === activityId);

    if (activity != null) {
      this.activity = activity;
    } else {
      // noinspection JSIgnoredPromiseFromCall
      this.router.navigate(['/four-oh-four']);
    }

    return subproject;
  }

  private newPrefilledActivity(subproject: Subproject, template: Activity){
    const newActivity = subproject.addActivity();
    newActivity.startDate = template.startDate;
    newActivity.endDate = template.endDate;
    newActivity.billabilityId = template.billabilityId;
    return newActivity;
  }
}
