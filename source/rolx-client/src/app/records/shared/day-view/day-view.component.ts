import { Component, Input } from '@angular/core';
import { Duration } from '@app/core/util/duration';
import { TimeOfDay } from '@app/core/util/time-of-day';
import { Record } from '@app/records/core/record';
import { RecordEntry } from '@app/records/core/record-entry';

@Component({
  selector: 'rolx-day-view',
  templateUrl: './day-view.component.html',
  styleUrls: ['./day-view.component.scss'],
})
export class DayViewComponent {
  private readonly defaultDayLength: Duration = Duration.fromHours(10);
  private readonly defaultStatTime: TimeOfDay = TimeOfDay.fromHours(8);
  private readonly dayLength: Duration = Duration.fromHours(24);

  private _record?: Record;

  recordEntries: RecordEntry[] = [];
  workDayLength: Duration = this.defaultDayLength;
  workDayStart: TimeOfDay = this.defaultStatTime;
  timeLabels: TimeOfDay[] = [];
  date?: moment.Moment;

  get record() {
    return this._record;
  }

  @Input()
  set record(value: Record | undefined) {
    this._record = value;
    this.update();
  }

  private update() {
    this.date = this._record?.date;
    this.recordEntries = this._record?.entries.filter((e) => e.begin) ?? [];

    if (this.recordEntries.length > 0) {
      let workStart = this.recordEntries[0].begin ?? this.defaultStatTime;
      workStart = TimeOfDay.fromHours(Math.floor(workStart.hours));

      const lastEntry = this.recordEntries[this.recordEntries.length - 1];
      let workLength = lastEntry.begin?.sub(workStart).add(lastEntry.grossDuration);
      workLength = Duration.fromHours(Math.ceil(workLength?.hours ?? 0));
      this.workDayLength =
        workLength.hours > this.defaultDayLength.hours ? workLength : this.defaultDayLength;

      const hoursWordayEndToMidnight = this.dayLength
        .sub(this.workDayLength)
        .sub(new Duration(workStart.seconds));
      if (hoursWordayEndToMidnight.hours < 0) {
        workStart = workStart.add(hoursWordayEndToMidnight);
      }
      this.workDayStart = workStart;
    } else {
      this.workDayStart = this.defaultStatTime;
      this.workDayLength = this.defaultDayLength;
    }
    this.timeLabels = this.getTimeLabels();
  }

  private getTimeLabels() {
    const timeLabels: TimeOfDay[] = [];
    const endTime = this.workDayStart.add(this.workDayLength);
    const increment = Duration.fromHours(1);
    let currentTime = this.workDayStart;

    while (currentTime.hours <= endTime.hours) {
      timeLabels.push(currentTime);
      currentTime = currentTime.add(increment);
    }
    return timeLabels;
  }
}
