import { Component, Input } from '@angular/core';
import { Duration } from '@app/core/util/duration';
import { TimeOfDay } from '@app/core/util/time-of-day';
import { RecordEntry } from '@app/records/core/record-entry';
import ColorHash from 'color-hash';

@Component({
  selector: 'rolx-day-view-entry',
  templateUrl: './day-view-entry.component.html',
  styleUrls: ['./day-view-entry.component.scss'],
})
export class DayViewEntryComponent {
  private colorHash = new ColorHash();

  @Input()
  recordEntry!: RecordEntry;

  @Input()
  workDayLength: Duration = Duration.fromHours(8.4);

  @Input()
  workDayStart: TimeOfDay = TimeOfDay.fromHours(8);

  get height() {
    const fractionOfTheDay = this.recordEntry.grossDuration.hours / this.workDayLength.hours;
    const test = fractionOfTheDay * 100 + '%';

    return test;
  }

  get top() {
    const offsetHours = this.recordEntry.begin?.sub(this.workDayStart)?.hours ?? 0;
    const offsetFraction = offsetHours / this.workDayLength.hours;
    const test = offsetFraction * 100 + '%';
    return test;
  }

  get backgroundColor() {
    return this.colorHash.hex(this.recordEntry.fullActivityNumber);
  }
}
