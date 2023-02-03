import { Routes } from '@angular/router';
import { RoleGuard } from '@app/auth/core/role.guard';
import { ActivityEditPageComponent } from '@app/projects/pages/activity-edit-page/activity-edit-page.component';
import { SubprojectActivitiesPageComponent } from '@app/projects/pages/subproject-activities-page/subproject-activities-page.component';
import { SubprojectEditPageComponent } from '@app/projects/pages/subproject-edit-page/subproject-edit-page.component';
import { SubprojectListPageComponent } from '@app/projects/pages/subproject-list-page/subproject-list-page.component';
import { SubprojectRecordsPageComponent } from '@app/projects/pages/subproject-records-page/subproject-records-page.component';
import { Role } from '@app/users/core/role';

export const ProjectsRoutes: Routes = [
  {
    path: 'subproject',
    component: SubprojectListPageComponent,
  },
  {
    path: 'subproject/:id/activity',
    component: SubprojectActivitiesPageComponent,
  },
  {
    path: 'subproject/:id/record',
    component: SubprojectRecordsPageComponent,
  },
  {
    path: 'subproject/:id/edit',
    component: SubprojectEditPageComponent,
    canActivate: [RoleGuard],
    data: { minRole: Role.Supervisor },
  },
  {
    path: 'subproject/:id/activity/:activityId',
    component: ActivityEditPageComponent,
    canActivate: [RoleGuard],
    data: { minRole: Role.Supervisor },
  },
];
