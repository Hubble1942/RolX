import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { momentEquals } from '@app/core/util/utils';
import * as moment from 'moment';

export class ReportRange {
  reportRangeType: string;

  @TransformAsIsoDate()
  customStart?: moment.Moment;

  @TransformAsIsoDate()
  customEnd?: moment.Moment;

  constructor(reportRangeType: string, customStart?: moment.Moment, customEnd?: moment.Moment) {
    this.reportRangeType = reportRangeType;
    this.customStart = customStart;
    this.customEnd = customEnd;
  }

  static equals(a?: ReportRange, b?: ReportRange): boolean {
    if (a === undefined && b === undefined) {
      return true;
    }
    if (a === undefined || b === undefined) {
      return false;
    }

    return (
      a.reportRangeType === b.reportRangeType &&
      momentEquals(a.customStart, b.customStart) &&
      momentEquals(a.customEnd, b.customEnd)
    );
  }
}
