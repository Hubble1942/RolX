import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import * as moment from 'moment';

export class Activity {
  id!: number;
  number!: number;
  name!: string;

  @TransformAsIsoDate()
  startDate!: moment.Moment;

  @TransformAsIsoDate()
  endDate?: moment.Moment;

  billabilityId!: number;
  billabilityName!: string;
  isBillable!: boolean;

  @TransformAsDuration()
  budget!: Duration;

  @TransformAsDuration()
  actual!: Duration;

  fullNumber!: string;
  fullName!: string;

  validateModel(): void {
    assertDefined(this, 'id');
    assertDefined(this, 'number');
    assertDefined(this, 'name');
    assertDefined(this, 'startDate');
    assertDefined(this, 'billabilityId');
    assertDefined(this, 'billabilityName');
    assertDefined(this, 'isBillable');
    assertDefined(this, 'budget');
    assertDefined(this, 'actual');
    assertDefined(this, 'fullNumber');
    assertDefined(this, 'fullName');
  }

  isOpenAt(date: moment.Moment): boolean {
    return date >= this.startDate && (this.endDate == null || date <= this.endDate);
  }
}
