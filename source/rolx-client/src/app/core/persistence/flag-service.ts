import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export type Flag =
  | 'asTree'
  | 'showColumn-'
  | 'showToggle'
  | 'showWeekends'
  | 'voiceInput'
  | 'formatDurationsAsDecimal';

@Injectable({
  providedIn: 'root',
})
export class FlagService {
  private static readonly Key = 'rolx-flag-service';
  private readonly data: { [flag: string]: boolean } = FlagService.Load();
  private readonly flagChangedSubject = new Subject<{ flag: Flag; state: boolean }>();

  readonly flagChanged$ = this.flagChangedSubject.asObservable();

  get(flag: Flag, defaultState: boolean): boolean {
    const state = this.data[flag];
    return state ?? defaultState;
  }

  set(flag: Flag, state: boolean) {
    this.data[flag] = state;
    this.save();

    this.flagChangedSubject.next({ flag, state });
  }

  private static Load(): { [flag: string]: boolean } {
    const text = localStorage.getItem(FlagService.Key);
    return text ? JSON.parse(text) : {};
  }

  private save() {
    localStorage.setItem(FlagService.Key, JSON.stringify(this.data));
  }
}
