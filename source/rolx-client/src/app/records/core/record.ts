import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Type } from 'class-transformer';
import * as moment from 'moment';

import { DayType } from './day-type';
import { PaidLeaveType } from './paid-leave-type';
import { RecordEntry } from './record-entry';

export class Record {
  @TransformAsIsoDate()
  date!: moment.Moment;

  userId!: string;
  dayType: DayType = DayType.Workday;
  dayName = '';

  @TransformAsDuration()
  nominalWorkTime = Duration.Zero;

  paidLeaveType?: PaidLeaveType;
  paidLeaveReason?: string;

  @Type(() => RecordEntry)
  entries: RecordEntry[] = [];

  validateModel(): void {
    assertDefined(this, 'date');
    assertDefined(this, 'userId');

    this.entries.forEach((e) => e.validateModel());
  }

  get totalDuration(): Duration {
    return new Duration(this.entries.reduce((sum, e) => sum + e.duration.seconds, 0));
  }

  get overtime(): Duration {
    return this.totalDuration.sub(this.nominalWorkTime);
  }

  get tooltip(): string {
    if (this.paidLeaveType != null) {
      return this.paidLeaveReason ?? '';
    }

    return this.overtime.toString(true);
  }

  get isComplete(): boolean {
    return this.totalDuration.isGreaterThanOrEqualTo(this.nominalWorkTime);
  }

  get isWorkday(): boolean {
    return this.dayType === DayType.Workday;
  }

  get isToday(): boolean {
    return this.date.isSame(moment(), 'day');
  }

  get mayHavePaidLeave(): boolean {
    return this.isWorkday && !this.isComplete;
  }

  addEntry(entry: RecordEntry) {
    this.entries.push(entry);
    this.entries = this.sortByBegin(this.entries);
  }

  hasEntriesOf(activity: Activity): boolean
  {
    return this.entries.some((e) => e.activityId === activity.id);
  }

  entriesOf(activity: Activity): RecordEntry[] {
    return this.sortByBegin(this.entries.filter((e) => e.activityId === activity.id));
  }

  moveEntriesOfActivity(fromActivity: Activity, toActivity: Activity): Record {
    const entries = this.entriesOf(fromActivity).map((e) => e.clone());

    // Need to remove first so as not to trip mayHavePaidLeave
    return this.removeEntriesOfActivity(fromActivity)
                .replaceEntriesOfActivity(toActivity, entries);
  }

  replaceEntriesOfActivity(activity: Activity, entries: RecordEntry[]): Record {
    const clone = this.removeEntriesOfActivity(activity);

    entries = entries.filter((e) => !e.duration.isZero);
    entries.forEach((e) => (e.activityId = activity.id));

    clone.entries = clone.sortByBegin(
      clone.entries.concat(...entries),
    );

    if (!clone.mayHavePaidLeave) {
      clone.paidLeaveType = undefined;
      clone.paidLeaveReason = undefined;
    }

    return clone;
  }

  removeEntriesOfActivity(activity: Activity): Record {
    const clone = this.clone();

    clone.entries = this.entries.filter((e) => e.activityId !== activity.id);

    return clone;
  }

  updatePaidLeaveType(
    paidLeaveType: PaidLeaveType | undefined,
    reason: string | undefined,
  ): Record {
    const clone = this.clone();

    clone.paidLeaveType = paidLeaveType;
    clone.paidLeaveReason = reason;

    return clone;
  }

  getTotalDurationOf(activity: Activity): Duration
  {
    return new Duration(this.entriesOf(activity).reduce((sum, e) => sum + e.duration.seconds, 0));
  }

  clone(): Record {
    const clone = new Record();
    Object.assign(clone, this);
    clone.entries = this.entries.map((e) => e.clone());

    return clone;
  }

  private sortByBegin(entries: RecordEntry[]): RecordEntry[] {
    return entries.sort((a, b) => {
      if (a.begin != null && b.begin != null) {
        return a.begin.sub(b.begin).seconds;
      }

      if (a.begin != null && b.begin == null) {
        return -1;
      }

      if (a.begin == null && b.begin != null) {
        return 1;
      }

      // both nullish
      return 0;
    });
  }
}
