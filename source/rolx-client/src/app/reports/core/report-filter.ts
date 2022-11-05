import { DateRange } from '@app/reports/core/date-range';

export class ReportFilter {
  dateRange!: DateRange;
  projectNumber?: number;
  subprojectNumber?: number;
  userIds!: string[];
  commentFilter?: string;
}
