import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PendingRequestInterceptor } from '@app/core/pending-request/pending-request.interceptor';
import { mapPlainToInstance, mapPlainToInstances } from '@app/core/util/operators';
import { Subproject } from '@app/projects/core/subproject';
import { environment } from '@env/environment';
import { Observable, shareReplay, tap } from 'rxjs';

import { Report } from './report';
import { ReportRange } from './report-range';
import { ReportRangeType } from './report-range-type';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private static readonly Url = environment.apiBaseUrl + '/v1/report';

  reportRangeTypes$ = this.httpClient
    .get<ReportRangeType[]>(ReportService.Url + '/rangeTypes')
    .pipe(
      mapPlainToInstances(ReportRangeType),
      tap((r) => r.forEach((x) => x.validateModel())),
      shareReplay(1),
    );

  constructor(private httpClient: HttpClient) {}

  getSubprojectReport(subproject: Subproject, range: ReportRange): Observable<Report> {
    let params = new HttpParams();
    params = params
      .append('subprojectId', subproject.id)
      .append('reportRangeType', range.reportRangeType);

    if (range.customStart !== undefined) {
      params = params.append('customStart', range.customStart.format('YYYY-MM-DD') ?? '');
    }

    if (range.customEnd !== undefined) {
      params = params.append('customEnd', range.customEnd.format('YYYY-MM-DD') ?? '');
    }

    return this.httpClient
      .get(ReportService.Url, {
        params,
        headers: {
          [PendingRequestInterceptor.NO_REDIRECT_ON_ERROR_HEADER]: 'foo',
        },
      })
      .pipe(
        mapPlainToInstance(Report),
        tap((r) => r.validateModel()),
      );
  }
}
