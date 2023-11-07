export class DurationBase<T extends DurationBase<T>> {
  constructor(public readonly seconds: number = 0) {}

  get hours(): number {
    return this.seconds / DurationBase.SecondsPerHour;
  }

  get isValid(): boolean {
    return !Number.isNaN(this.seconds);
  }

  isSame(other: T): boolean {
    return other && this.seconds === other.seconds;
  }

  protected static readonly SecondsPerHour = 3600;
  protected static readonly SecondsPerMinute = 60;
  protected static readonly SecondsPerDay = 24 * DurationBase.SecondsPerHour;
}
