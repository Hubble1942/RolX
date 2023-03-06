import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppImportModule } from '@app/app-import.module';
import { ProgressBarComponent } from '@app/core/progress-bar/progress-bar.component';
import { ActivityEditPageComponent } from '@app/projects/pages/activity-edit-page/activity-edit-page.component';
import { SubprojectActivitiesPageComponent } from '@app/projects/pages/subproject-activities-page/subproject-activities-page.component';
import { SubprojectEditPageComponent } from '@app/projects/pages/subproject-edit-page/subproject-edit-page.component';
import { SubprojectListPageComponent } from '@app/projects/pages/subproject-list-page/subproject-list-page.component';
import { SubprojectRecordsPageComponent } from '@app/projects/pages/subproject-records-page/subproject-records-page.component';
import { ActivityFormComponent } from '@app/projects/shared/activity/form/activity-form.component';
import { StarredActivityIndicatorComponent } from '@app/projects/shared/activity/starred-indicator/starred-activity-indicator.component';
import { ActivityTableComponent } from '@app/projects/shared/activity/table/activity-table.component';
import { SubprojectFormComponent } from '@app/projects/shared/subproject/form/subproject-form.component';
import { SubprojectLayoutPageComponent } from '@app/projects/shared/subproject/page-layout/subproject-page-layout.component';
import { SubprojectTableComponent } from '@app/projects/shared/subproject/table/subproject-table.component';
import { ReportsModule } from '@app/reports/reports.module';

@NgModule({
  imports: [AppImportModule, CommonModule, ReportsModule],
  declarations: [
    ActivityEditPageComponent,
    ActivityFormComponent,
    ActivityTableComponent,
    ProgressBarComponent,
    StarredActivityIndicatorComponent,
    SubprojectActivitiesPageComponent,
    SubprojectEditPageComponent,
    SubprojectFormComponent,
    SubprojectLayoutPageComponent,
    SubprojectListPageComponent,
    SubprojectRecordsPageComponent,
    SubprojectTableComponent,
  ],
  exports: [
    ActivityEditPageComponent,
    StarredActivityIndicatorComponent,
    SubprojectActivitiesPageComponent,
    SubprojectEditPageComponent,
    SubprojectLayoutPageComponent,
    SubprojectListPageComponent,
    SubprojectRecordsPageComponent,
  ],
})
export class ProjectsModule {}
