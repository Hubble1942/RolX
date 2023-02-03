import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import { Type } from 'class-transformer';
import * as moment from 'moment';

import { ReportEntry } from './report-entry';

export class Report {
  subproject!: string;

  @TransformAsIsoDate()
  startDate!: moment.Moment;

  @TransformAsIsoDate()
  endDate!: moment.Moment;

  @Type(() => ReportEntry)
  reportEntries: ReportEntry[] = [];

  validateModel(): void {
    assertDefined(this, 'subproject');
    assertDefined(this, 'startDate');
    assertDefined(this, 'endDate');

    this.reportEntries.forEach((re) => re.validateModel());
  }
}
