import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export type Flag =
  | 'asTree'
  | 'showColumn-'
  | 'showToggle'
  | 'showWeekends'
  | 'voiceInput'
  | 'formatDurationsAsDecimal'
  | 'showDayView';

@Injectable({
  providedIn: 'root',
})
export class FlagService {
  private static readonly Key = 'rolx-flag-service';
  private readonly data: { [flag: string]: boolean } = FlagService.Load();
  private readonly flagChangedSubject = new Subject<{ flag: Flag; state: boolean }>();

  readonly flagChanged$ = this.flagChangedSubject.asObservable();

  get(flag: Flag): boolean {
    return this.data[flag] ?? false;
  }

  set(flag: Flag, state: boolean) {
    this.data[flag] = state;
    this.save();

    this.flagChangedSubject.next({ flag, state });
  }

  private static Load(): { [flag: string]: boolean } {
    const text = localStorage.getItem(FlagService.Key);
    const data = text ? JSON.parse(text) : {};

    data.showColumn_startDate ??= true;
    data.showColumn_endDate ??= true;
    data.showColumn_budgetTime ??= true;
    data.showColumn_plannedTime ??= true;
    data.showColumn_actualTime ??= true;
    data.showColumn_isBillable ??= true;
    data.showColumn_budgetConsumed ??= true;
    data.showColumn_plannedConsumed ??= true;
    data.showDayView ??= true;

    return data;
  }

  private save() {
    localStorage.setItem(FlagService.Key, JSON.stringify(this.data));
  }
}
