import { Injectable } from '@angular/core';
import * as moment from 'moment';
import { Moment } from 'moment';

const START_TIME_KEY = 'toggle-start-time';

@Injectable({
  providedIn: 'root',
})
export class ToggleService {
  startToggle(startTime: Moment): void {
    localStorage.setItem(START_TIME_KEY, startTime.toISOString());
  }

  clearToggle() {
    localStorage.removeItem(START_TIME_KEY);
  }

  get startTime(): Moment | undefined {
    const startTimeFromLocalStorage = localStorage.getItem(START_TIME_KEY);
    if (startTimeFromLocalStorage) {
      return moment(startTimeFromLocalStorage);
    }
    return undefined;
  }

  get active(): boolean {
    return localStorage.getItem(START_TIME_KEY) !== null;
  }
}
