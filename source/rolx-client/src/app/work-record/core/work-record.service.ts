import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorResponse } from '@app/core/error';
import { IsoDate, mapPlainToClassArray } from '@app/core/util';
import { environment } from '@env/environment';
import { classToPlain } from 'class-transformer';
import moment from 'moment';
import { Observable, ReplaySubject, throwError } from 'rxjs';
import { catchError, mapTo, switchMap, tap } from 'rxjs/operators';
import { Record } from './record';

const WorkRecordUrl = environment.apiBaseUrl + '/v1/workrecord';

@Injectable({
  providedIn: 'root',
})
export class WorkRecordService {
  updateSequence = new ReplaySubject<number>(1);

  constructor(private httpClient: HttpClient) {
    console.log('--- WorkRecordService.ctor()');

    this.updateSequence.next(1);
  }

  private static UrlWithId(id: number) {
    return WorkRecordUrl + '/' + id;
  }

  getMonth(month: moment.Moment): Observable<Record[]> {
    const url = WorkRecordUrl + '/month/' + month.format('YYYY-MM');
    return this.httpClient.get(url).pipe(
      mapPlainToClassArray(Record),
    );
  }

  getRange(begin: moment.Moment, end: moment.Moment): Observable<Record[]> {
    const url = WorkRecordUrl
      + '/range/'
      + IsoDate.fromMoment(begin)
      + '..'
      + IsoDate.fromMoment(end);

    return this.httpClient.get<any[]>(url).pipe(
      mapPlainToClassArray(Record),
    );
  }

  update(record: Record): Observable<Record> {
    const currentSequence = this.updateSequence;
    const nextSequence = new ReplaySubject<number>(1);
    this.updateSequence = nextSequence;

    return currentSequence.pipe(
      switchMap(() => record.id === 0 ? this.internalCreate(record) : this.internalUpdate(record)),
      tap(() => nextSequence.next(0)),
      catchError(e => {
        nextSequence.next(0);
        return throwError(e);
      }),
    );
  }

  private internalUpdate(record: Record): Observable<Record> {
    return this.httpClient.put(WorkRecordService.UrlWithId(record.id), classToPlain(record)).pipe(
      mapTo(record),
      catchError(e => throwError(new ErrorResponse(e))),
    );
  }

  private internalCreate(record: Record): Observable<Record> {
    return this.httpClient.post<any>(WorkRecordUrl, classToPlain(record)).pipe(
      tap(result => record.id = result.id),
      mapTo(record),
      catchError(e => throwError(new ErrorResponse(e))),
    );
  }
}