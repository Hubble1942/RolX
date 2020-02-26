import { Duration, TransformAsDuration, TransformAsIsoDate } from '@app/core/util';
import { Phase } from '@app/projects/core';
import { Type } from 'class-transformer';
import moment from 'moment';
import { DayType } from './day-type';
import { PaidLeaveType } from './paid-leave-type';
import { RecordEntry } from './record-entry';

export class Record {

  @TransformAsIsoDate()
  date: moment.Moment;

  userId: string;
  dayType: DayType = DayType.Workday;
  dayName = '';

  @TransformAsDuration()
  nominalWorkTime = Duration.Zero;

  paidLeaveType: PaidLeaveType | null;
  paidLeaveReason: string | null;

  @Type(() => RecordEntry)
  entries: RecordEntry[] = [];

  get totalDuration(): Duration {
    return new Duration(
      this.entries.reduce(
        (sum, e) => sum + e.duration.seconds, 0));
  }

  get overtime(): Duration {
    return this.totalDuration.sub(this.nominalWorkTime);
  }

  get isComplete(): boolean {
    return this.totalDuration.isGreaterThanOrEqualTo(this.nominalWorkTime);
  }

  get isWorkday(): boolean {
    return this.dayType === DayType.Workday;
  }

  get mayHavePaidLeave(): boolean {
    return this.isWorkday && !this.isComplete;
  }

  entriesOf(phase: Phase): RecordEntry[] {
    return this.entries.filter(e => e.phaseId === phase.id);
  }

  replaceEntriesOfPhase(phase: Phase, entries: RecordEntry[]): Record {
    const clone = this.clone();

    entries = entries.filter(e => !e.duration.isZero);
    entries.forEach(e => e.phaseId = phase.id);

    clone.entries = this.entries
      .filter(e => e.phaseId !== phase.id)
      .concat(...entries);

    if (!clone.mayHavePaidLeave) {
      clone.paidLeaveType = null;
      clone.paidLeaveReason = null;
    }

    return clone;
  }

  updatePaidLeaveType(paidLeaveType: PaidLeaveType | null, reason: string | null): Record {
    const clone = this.clone();

    clone.paidLeaveType = paidLeaveType;
    clone.paidLeaveReason = reason;

    return clone;
  }

  private clone(): Record {
    const clone = new Record();
    Object.assign(clone, this);

    return clone;
  }

}