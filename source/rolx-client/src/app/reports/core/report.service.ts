import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Report } from '@app/reports/core/report';
import { ReportFilter } from '@app/reports/core/report-filter';
import { environment } from '@env/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class ReportService {
  private static readonly Url = environment.apiBaseUrl + '/v1/report';

  constructor(private httpClient: HttpClient) {}

  get(filter: ReportFilter): Observable<Report> {
    let params = new HttpParams();

    params = params.append('begin', filter.dateRange.begin.format('YYYY-MM-DD'))
                   .append('end', filter.dateRange.end.format('YYYY-MM-DD'));

    if (filter.subprojectNumber != null) {
      params = params.append('subprojectNumber', filter.subprojectNumber);
    }

    if(filter.projectNumber != null) {
      params = params.append('projectNumber', filter.projectNumber);
    }

    if (!!filter.commentFilter){
      params = params.append('commentFilter', filter.commentFilter);
    }

    if (filter.userIds?.some(x => x)) {
      params = params.append('userIds', filter.userIds.join(','));
    }

    return this.httpClient
      .get<Report>(ReportService.Url, { params, responseType: 'json', observe: 'response' })
      .pipe(map((response) => {
        if (response.body == null) {
          throw new Error('No body received');
        }
        return response.body;
      }),
    );
  }
}
