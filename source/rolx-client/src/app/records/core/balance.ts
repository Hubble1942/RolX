import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import * as moment from 'moment';

export class Balance {
  @TransformAsIsoDate()
  byDate!: moment.Moment;

  @TransformAsDuration()
  overtime!: Duration;

  @TransformAsDuration()
  vacationAvailable!: Duration;

  @TransformAsDuration()
  vacationPlanned!: Duration;

  vacationAvailableDays!: number | null;
  vacationPlannedDays!: number | null;

  validateModel(): void {
    assertDefined(this, 'byDate');
    assertDefined(this, 'overtime');
    assertDefined(this, 'vacationAvailable');
    assertDefined(this, 'vacationAvailableDays');
    assertDefined(this, 'vacationPlannedDays');
    assertDefined(this, 'vacationPlanned');
  }
}
