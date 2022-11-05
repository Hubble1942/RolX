import { Component, Input } from '@angular/core';
import { Report } from '@app/reports/core/report';

@Component({
  selector: 'rolx-report-table',
  templateUrl: './report-table.component.html',
  styleUrls: ['./report-table.component.scss'],
})
export class ReportTableComponent {
    @Input()
    report!: Report;
}
