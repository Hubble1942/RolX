import { Component, OnInit } from '@angular/core';
import { DateRange } from '@app/reports/core/date-range';
import { Report } from '@app/reports/core/report';
import { ReportFilter } from '@app/reports/core/report-filter';
import { ReportService } from '@app/reports/core/report.service';
import * as moment from 'moment';

@Component({
  selector: 'rolx-report-page',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.scss'],
})
export class ReportPageComponent implements OnInit {
  report!: Report;
  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    const dateRange = new DateRange();
    dateRange.begin = moment('2022-10-01');
    dateRange.end = moment('2022-11-30');

    const filter = new ReportFilter();
    filter.dateRange = dateRange;
    filter.userIds = ['08da1d77-f7ea-40c4-8ea6-8fa4ca087781'];
    this.reportFilterChanged(filter);
  }

  reportFilterChanged(filter: ReportFilter): void {
    this.reportService.get(filter).subscribe((report) => (this.report = report));
    // TODO los
  }
}
