import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppImportModule } from '@app/app-import.module';
import { CoreModule } from '@app/core/core.module';
import { ProjectsModule } from '@app/projects/projects.module';

import { WeekPageComponent } from './pages/week-page/week-page.component';
import { YearOverviewPageComponent } from './pages/year-overview-page/year-overview-page.component';
import { BalanceIndicatorComponent } from './shared/balance-indicator/balance-indicator.component';
import { DayViewEntryComponent } from './shared/day-view-entry/day-view-entry.component';
import { DayViewComponent } from './shared/day-view/day-view.component';
import { DurationEditComponent } from './shared/duration-edit/duration-edit.component';
import { HolidayTableComponent } from './shared/holiday-table/holiday-table.component';
import { InvalidEntriesDialogComponent } from './shared/invalid-entries-dialog/invalid-entries-dialog.component';
import { MonthlyWorkTimeTableComponent } from './shared/monthly-work-time-table/monthly-work-time-table.component';
import { MultiEntriesDialogComponent } from './shared/multi-entries-dialog/multi-entries-dialog.component';
import { PaidLeaveSelectComponent } from './shared/paid-leave-select/paid-leave-select.component';
import { ReasonDialogComponent } from './shared/paid-leave-select/reason-dialog/reason-dialog.component';
import { ParserFailedDialogComponent } from './shared/parser-failed-dialog/parser-failed-dialog.component';
import { SetEditLockFormComponent } from './shared/set-edit-lock-form/set-edit-lock-form.component';
import { StartToggleComponent } from './shared/start-toggle/start-toggle.component';
import { StopToggleComponent } from './shared/stop-toggle/stop-toggle.component';
import { WeekTableCellComponent } from './shared/week-table/cell/week-table-cell.component';
import { WeekTableComponent } from './shared/week-table/week-table.component';

const exportedDeclarations = [
  BalanceIndicatorComponent,
  SetEditLockFormComponent,
  WeekPageComponent,
  YearOverviewPageComponent,
];

@NgModule({
  imports: [AppImportModule, CommonModule, CoreModule, ProjectsModule],
  declarations: [
    ...exportedDeclarations,
    DayViewComponent,
    DayViewEntryComponent,
    DurationEditComponent,
    HolidayTableComponent,
    InvalidEntriesDialogComponent,
    MonthlyWorkTimeTableComponent,
    MultiEntriesDialogComponent,
    PaidLeaveSelectComponent,
    ParserFailedDialogComponent,
    ReasonDialogComponent,
    StartToggleComponent,
    StopToggleComponent,
    WeekTableCellComponent,
    WeekTableComponent,
  ],
  exports: exportedDeclarations,
})
export class RecordsModule {}
