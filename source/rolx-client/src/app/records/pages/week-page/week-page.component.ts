import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '@app/auth/core/auth.service';
import { Flag, FlagService } from '@app/core/persistence/flag-service';
import { VoiceService } from '@app/core/voice/voice.service';
import { Activity } from '@app/projects/core/activity';
import { ActivityService } from '@app/projects/core/activity.service';
import { FavouriteActivityService } from '@app/projects/core/favourite-activity.service';
import { EditLockService } from '@app/records/core/edit-lock.service';
import { Record } from '@app/records/core/record';
import { WorkRecordService } from '@app/records/core/work-record.service';
import { WeekPageParams } from '@app/records/pages/week-page/week-page-params';
import { User } from '@app/users/core/user';
import { UserService } from '@app/users/core/user.service';
import { Moment } from 'moment';
import {
  BehaviorSubject,
  combineLatest,
  forkJoin,
  NEVER,
  Observable,
  of,
  withLatestFrom,
} from 'rxjs';
import { catchError, debounceTime, map, startWith, switchMap, tap } from 'rxjs/operators';

@Component({
  selector: 'rolx-week-page',
  templateUrl: './week-page.component.html',
  styleUrls: ['./week-page.component.scss'],
})
export class WeekPageComponent {
  private readonly recordsChangedSubject = new BehaviorSubject<void>(undefined);

  readonly filterControl = new FormControl();
  readonly filterText$ = this.filterControl.valueChanges.pipe(debounceTime(200), startWith(''));

  readonly routeParams$: Observable<WeekPageParams> = this.route.url.pipe(
    withLatestFrom(this.route.paramMap, this.route.queryParamMap),
    map(([, paramMap, queryParamMap]) =>
      WeekPageParams.evaluate(
        paramMap,
        queryParamMap,
        this.authService.currentApprovalOrError.user.id,
      ),
    ),
    catchError(() => {
      // noinspection JSIgnoredPromiseFromCall
      this.router.navigate(['four-oh-four']);
      return NEVER;
    }),
  );

  readonly recordsAndSuitable$ = combineLatest([
    this.routeParams$,
    this.recordsChangedSubject,
  ]).pipe(
    switchMap(([params]) =>
      forkJoin([
        this.workRecordService.getRange(params.userId, params.monday, params.nextMonday),
        this.activityService.getSuitable(params.userId, params.monday),
      ]),
    ),
  );

  readonly userAllActivitiesParams$ = this.routeParams$.pipe(
    tap(() => this.editLockService.refresh()),
    switchMap((params) =>
      forkJoin([
        params.isCurrentUser
          ? of(this.authService.currentApprovalOrError.user)
          : this.userService.getById(params.userId),
        this.activityService.getAll(params.monday),
        of(params),
      ]),
    ),
  );

  readonly recordsActivitiesUser$: Observable<[Record[], Activity[], User, boolean]> =
    combineLatest([
      this.filterText$,
      this.recordsAndSuitable$,
      this.userAllActivitiesParams$,
      this.favouriteActivityService.favourites$,
    ]).pipe(
      map(([filterText, [records, suitable], [user, all, params], fav]) => {
        const activities: Activity[] =
          filterText === ''
            ? this.filterByDate(
                params.monday,
                params.nextMonday,
                suitable.concat(
                  params.isCurrentUser
                    ? fav.filter((item) => !suitable.some((a) => a.id === item.id))
                    : [],
                ),
                records,
              )
            : this.filterByName(filterText, all);
        return [records, activities, user, filterText !== ''];
      }),
    );

  get showWeekends() {
    return this.flagService.get('showWeekends', false);
  }

  get asTree() {
    return this.flagService.get('asTree', false);
  }

  get showToggle() {
    return this.flagService.get('showToggle', false);
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private workRecordService: WorkRecordService,
    private activityService: ActivityService,
    private userService: UserService,
    private authService: AuthService,
    private editLockService: EditLockService,
    private flagService: FlagService,
    public voiceService: VoiceService,
    private favouriteActivityService: FavouriteActivityService,
  ) {}

  setFilter(value: string) {
    this.filterControl.setValue(value);
  }

  filterByDate(startDate: Moment, endDate: Moment, activities: Activity[], records: Record[]) {
    return activities.filter(
      (activity) =>
        (activity.startDate.isBefore(startDate) &&
          (activity.endDate == null || activity.endDate.isSameOrAfter(endDate))) ||
        records.some((r) => r.entries.some((e) => e.activityId === activity.id)),
    );
  }

  filterByName(filterText: string, activities: Activity[]) {
    const filterParts = filterText.trim().toLocaleLowerCase().split(' ');
    return activities.filter((activity) => {
      const activityName = activity.fullName.toLocaleLowerCase();
      return filterParts.every((f) => activityName.includes(f));
    });
  }

  selectText(event: FocusEvent): void {
    setTimeout(() => {
      if (event.target instanceof HTMLInputElement) {
        event.target.select();
      }
    });
  }

  checkBoxForFlag(flag: Flag): string {
    return this.flagService.get(flag, false) ? 'check_box' : 'check_box_outline_blank';
  }

  toggleFlag(flag: Flag) {
    this.flagService.set(flag, !this.flagService.get(flag, false));
  }

  notifyRecordsChanged() {
    this.recordsChangedSubject.next();
  }
}
