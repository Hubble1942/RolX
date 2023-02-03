import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppImportModule } from '@app/app-import.module';
import { CoreModule } from '@app/core/core.module';
import { ReportTableComponent } from '@app/reports/shared/report/table/report-table.component';

import { MonthFormatDirective } from './core/month-format.directive';
import { ExportPageComponent } from './pages/export-page/export-page.component';
import { UserMonthReportPageComponent } from './pages/user-month-report-page/user-month-report-page.component';
import { DaysIndicatorComponent } from './shared/days-indicator/days-indicator.component';
import { ExportMonthCardComponent } from './shared/export-month-card/export-month-card.component';
import { ExportRangeCardComponent } from './shared/export-range-card/export-range-card.component';
import { ReportRangeComponent } from './shared/report/range/report-range.component';

const exportedComponents = [
  DaysIndicatorComponent,
  ExportMonthCardComponent,
  ExportPageComponent,
  ExportRangeCardComponent,
  MonthFormatDirective,
  ReportRangeComponent,
  ReportTableComponent,
  UserMonthReportPageComponent,
];

@NgModule({
  declarations: exportedComponents,
  exports: exportedComponents,
  imports: [AppImportModule, CommonModule, CoreModule],
})
export class ReportsModule {}
