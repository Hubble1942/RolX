import { assertDefined } from '@app/core/util/utils';
import { DateRange } from '@app/reports/core/date-range';
import { ReportEntry } from '@app/reports/core/report-entry';
import { Type } from 'class-transformer';

export class Report {
  subproject!: string;

  @Type(() => DateRange)
  range!: DateRange;

  @Type(() => ReportEntry)
  entries!: ReportEntry[];

  validateModel(): void {
    assertDefined(this, 'subproject');

    assertDefined(this, 'range');
    this.range.validateModel();

    assertDefined(this, 'entries');
    this.entries.forEach((entry) => entry.validateModel());
  }
}
