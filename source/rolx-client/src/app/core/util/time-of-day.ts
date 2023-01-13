import { Transform } from 'class-transformer';
import { Moment } from 'moment';

import { Duration } from './duration';
import { DurationBase } from './duration.base';

export class TimeOfDay extends DurationBase<TimeOfDay> {
  static readonly Zero = new TimeOfDay();
  static readonly ColonPattern =
    /^([0-1]?\d|2[0-3]|24(?=(?::|\.\.|,,)?0{0,2}))?(?:(?::|\.\.|,,)([0-5]?\d)?)?(([0-1]?\d|2[0-3])?[.,]\d*|24[.,]0*)?$/;
  static readonly FourDigitsPattern = /^([0-1]\d|2[0-3]|24(?=00))([0-5]\d)$/;
  static readonly PatternGroups = {
    Hours: 1,
    Minutes: 2,
    DecimalHours: 3,
  };

  static fromMoment(moment: Moment) {
    const seconds =
      moment.hours() * DurationBase.SecondsPerHour +
      moment.minutes() * DurationBase.SecondsPerMinute;
    return new TimeOfDay(seconds);
  }

  static fromHours(hours: number) {
    return new TimeOfDay(Math.round(hours * DurationBase.SecondsPerHour));
  }

  static parse(time: string | any, zeroIfEmpty: boolean = false): TimeOfDay {
    if (time instanceof TimeOfDay) {
      return time as TimeOfDay;
    }

    if (typeof time === 'string') {
      time = time.trim();
    }

    if (zeroIfEmpty && (time == null || time === '')) {
      return TimeOfDay.Zero;
    }

    const match = this.ColonPattern.exec(time) || this.FourDigitsPattern.exec(time);
    if (!match) {
      return new TimeOfDay(NaN);
    }

    if (match[TimeOfDay.PatternGroups.DecimalHours]) {
      return TimeOfDay.fromHours(Number.parseFloat(time.replace(',', '.')));
    }

    const hoursGroup = match[TimeOfDay.PatternGroups.Hours];
    const hours = hoursGroup ? Number.parseInt(hoursGroup, 10) : 0;

    const minutesGroup = match[TimeOfDay.PatternGroups.Minutes];
    const minutes = minutesGroup ? Number.parseInt(minutesGroup, 10) : 0;

    const seconds = hours * DurationBase.SecondsPerHour + minutes * DurationBase.SecondsPerMinute;
    return new TimeOfDay(seconds);
  }

  override get isValid(): boolean {
    return super.isValid && this.seconds >= 0 && this.seconds <= DurationBase.SecondsPerDay;
  }

  get isZero(): boolean {
    return this.isSame(TimeOfDay.Zero);
  }

  add(duration: Duration): TimeOfDay {
    return new TimeOfDay(this.seconds + duration.seconds);
  }

  sub(other: TimeOfDay): Duration {
    return new Duration(this.seconds - other.seconds);
  }
}

export const TransformAsTimeOfDay = (): ((target: any, key: string) => void) => {
  const toClass = Transform(({ value }) => (value != null ? new TimeOfDay(value) : undefined), {
    toClassOnly: true,
  });
  const toPlain = Transform(({ value }) => value?.seconds, { toPlainOnly: true });

  return (target: any, key: string) => {
    toClass(target, key);
    toPlain(target, key);
  };
};
