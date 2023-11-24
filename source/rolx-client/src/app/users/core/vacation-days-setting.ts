import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import * as moment from 'moment';

export class VacationDaysSetting {
  @TransformAsIsoDate()
  startDate!: moment.Moment;

  vacationDays!: number;

  validateModel(): void {
    assertDefined(this, 'startDate');
    assertDefined(this, 'vacationDays');
  }
}
