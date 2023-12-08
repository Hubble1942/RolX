import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppImportModule } from '@app/app-import.module';
import { UserEditPageComponent } from '@app/users/pages/user-edit-page/user-edit-page.component';
import { UserListPageComponent } from '@app/users/pages/user-list-page/user-list-page.component';
import { PartTimeSettingsFormComponent } from '@app/users/shared/part-time-settings-form/part-time-settings-form.component';
import { UserAvatarComponent } from '@app/users/shared/user/avatar/user-avatar.component';
import { UserFormComponent } from '@app/users/shared/user/form/user-form.component';
import { UserMenuComponent } from '@app/users/shared/user/menu/user-menu.component';
import { UserTableComponent } from '@app/users/shared/user/table/user-table.component';

import { BalanceCorrectionsFormComponent } from './shared/balance-corrections-form/balance-corrections-form.component';
import { VacationDaysSettingsFormComponent } from './shared/vacation-days-settings-form/vacation-days-settings-form.component';

@NgModule({
  imports: [AppImportModule, CommonModule],
  exports: [UserAvatarComponent, UserMenuComponent],
  declarations: [
    BalanceCorrectionsFormComponent,
    PartTimeSettingsFormComponent,
    UserAvatarComponent,
    UserEditPageComponent,
    UserFormComponent,
    UserListPageComponent,
    UserMenuComponent,
    UserTableComponent,
    VacationDaysSettingsFormComponent,
  ],
})
export class UsersModule {}
