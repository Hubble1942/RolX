import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/auth/core/auth.service';
import { FileSaverService } from '@app/core/util/file-saver.service';
import { Subproject } from '@app/projects/core/subproject';
import { SubprojectService } from '@app/projects/core/subproject.service';
import { ExportService } from '@app/reports/core/export.service';
import { Report } from '@app/reports/core/report';
import { ReportRange } from '@app/reports/core/report-range';
import { ReportService } from '@app/reports/core/report.service';
import { plainToInstance } from 'class-transformer';
import { combineLatest, lastValueFrom, BehaviorSubject, throwError, of } from 'rxjs';
import { catchError, filter, mergeMap, shareReplay, switchMap } from 'rxjs/operators';

@Component({
  selector: 'rolx-subproject-records-page',
  templateUrl: './subproject-records-page.component.html',
  styleUrls: ['./subproject-records-page.component.scss'],
})
export class SubprojectRecordsPageComponent implements OnInit {
  static readonly REPORT_RANGE_KEY = 'rolx-report-range';

  readonly mayEdit = this.authService.currentApprovalOrError.isSupervisor;
  readonly mayExport = this.authService.currentApprovalOrError.isSupervisor;

  readonly subproject$ = this.route.paramMap.pipe(
    switchMap((params) => this.subprojectService.getById(Number(params.get('id')))),
    catchError((e) => {
      if (e.status === 404) {
        // noinspection JSIgnoredPromiseFromCall
        this.router.navigate(['/four-oh-four']);
      }

      return throwError(() => e);
    }),
    shareReplay(1),
  );

  range$ = new BehaviorSubject<ReportRange | undefined>(undefined);
  report$ = combineLatest({ range: this.range$, subproject: this.subproject$ }).pipe(
    filter(({ range }) => range !== undefined),
    mergeMap(({ range, subproject }) =>
      this.reportService
        .getSubprojectReport(subproject, range as any)
        .pipe(catchError((e) => of(undefined))),
    ),
  );

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly subprojectService: SubprojectService,
    private readonly authService: AuthService,
    private exportService: ExportService,
    private fileSaverService: FileSaverService,
    private reportService: ReportService,
  ) {}

  ngOnInit(): void {
    const rangeString = localStorage.getItem(SubprojectRecordsPageComponent.REPORT_RANGE_KEY);
    if (rangeString !== null) {
      try {
        this.range$.next(plainToInstance(ReportRange, JSON.parse(rangeString) as ReportRange));
      } catch {
        // swallow
      }
    }
  }

  get range() {
    return this.range$.value;
  }

  set range(range: ReportRange | undefined) {
    this.range$.next(range);
    if (range !== undefined) {
      localStorage.setItem(SubprojectRecordsPageComponent.REPORT_RANGE_KEY, JSON.stringify(range));
    }
  }

  async export(subproject: Subproject, report: Report): Promise<void> {
    const data = await lastValueFrom(
      this.exportService.download(subproject, report.startDate, report.endDate),
    );
    this.fileSaverService.save(data);
  }
}
