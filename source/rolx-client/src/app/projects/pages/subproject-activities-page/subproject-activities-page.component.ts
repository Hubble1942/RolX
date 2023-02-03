import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/auth/core/auth.service';
import { FileSaverService } from '@app/core/util/file-saver.service';
import { Subproject } from '@app/projects/core/subproject';
import { SubprojectService } from '@app/projects/core/subproject.service';
import { ExportService } from '@app/reports/core/export.service';
import * as moment from 'moment';
import { lastValueFrom, Observable, throwError } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

@Component({
  selector: 'rolx-subproject-activities-page',
  templateUrl: './subproject-activities-page.component.html',
  styleUrls: ['./subproject-activities-page.component.scss'],
})
export class SubprojectActivitiesPageComponent {
  readonly mayEdit = this.authService.currentApprovalOrError.isSupervisor;
  readonly mayExport = this.authService.currentApprovalOrError.isSupervisor;

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

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly subprojectService: SubprojectService,
    private readonly authService: AuthService,
    private exportService: ExportService,
    private fileSaverService: FileSaverService,
  ) {}

  async exportLastMonth(subproject: Subproject) {
    const data = await lastValueFrom(
      this.exportService.download(
        subproject,
        moment().startOf('month').add(-1, 'days').startOf('month'),
      ),
    );
    this.fileSaverService.save(data);
  }

  private initializeSubproject(idText: string | null): Observable<Subproject> {
    return this.subprojectService.getById(Number(idText));
  }
}
