import { Component } from '@angular/core';
import { ReportFilter } from '@app/reports/core/report-filter';
import { ReportService } from '@app/reports/core/report.service';

@Component({
  selector: 'rolx-report-page',
  templateUrl: './report-page.component.html',
  styleUrls: ['./report-page.component.scss'],
})
export class ReportPageComponent {
  constructor(private reportService: ReportService) {}

  reportFilterChanged(filter: ReportFilter): void {
    const call = this.reportService.get(filter);
    // TODO los
  }
}
