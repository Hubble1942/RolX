import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import * as moment from 'moment';

export class DateRange {
  @TransformAsIsoDate()
  begin!: moment.Moment;

  @TransformAsIsoDate()
  end!: moment.Moment;

  validateModel(): void {
    assertDefined(this, 'begin');
    assertDefined(this, 'end');
  }
}
