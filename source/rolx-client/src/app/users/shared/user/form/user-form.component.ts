import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ErrorResponse } from '@app/core/error/error-response';
import { ErrorService } from '@app/core/error/error.service';
import { assertDefined } from '@app/core/util/utils';
import { PartTimeSetting } from '@app/users/core/part-time-setting';
import { Role } from '@app/users/core/role';
import { User } from '@app/users/core/user';
import { UserService } from '@app/users/core/user.service';
import { VacationDaysSetting } from '@app/users/core/vacation-days-setting';

@Component({
  selector: 'rolx-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  readonly Role = Role;

  readonly roleControl = new FormControl(null, Validators.required);
  readonly entryDateControl = new FormControl(null, Validators.required);
  readonly leavingDateControl = new FormControl({ value: null });
  readonly partTimeSettings = new FormArray([]);
  readonly vacationDaysSettings = new FormArray([]);
  readonly balanceCorrections = new FormArray([]);
  readonly form = new FormGroup({
    role: this.roleControl,
    entryDate: this.entryDateControl,
    leavingDate: this.leavingDateControl,
    partTimeSettings: this.partTimeSettings,
    vacationDaysSettings: this.vacationDaysSettings,
    balanceCorrections: this.balanceCorrections,
  });

  @Input()
  user!: User;

  constructor(
    private router: Router,
    private userService: UserService,
    private errorService: ErrorService,
  ) {}

  ngOnInit() {
    assertDefined(this, 'user');

    const { partTimeSettings, vacationDaysSettings } = this.user;
    if (
      !(
        partTimeSettings.length > 0 &&
        partTimeSettings[0].startDate.isSame(this.user.entryDate, 'day')
      )
    ) {
      // If there is no entry defining the beginning add one in front to keep array sorted
      const defaultEntry = new PartTimeSetting();
      defaultEntry.startDate = this.user.entryDate;
      defaultEntry.factor = 1;
      partTimeSettings.unshift(defaultEntry);
    }
    partTimeSettings.forEach((s) => (s.factor = Math.round(s.factor * 100)));

    if (
      !(
        vacationDaysSettings.length > 0 &&
        vacationDaysSettings[0].startDate.isSame(this.user.entryDate, 'day')
      )
    ) {
      const defaultVacation = new VacationDaysSetting();
      defaultVacation.startDate = this.user.entryDate;
      defaultVacation.vacationDays = 25;
      vacationDaysSettings.unshift(defaultVacation);
    }

    this.form.controls['entryDate'].valueChanges.subscribe((v) =>
      (this.partTimeSettings.controls[0] as FormGroup)?.controls['startDate'].setValue(v),
    );

    this.form.patchValue(this.user);
  }

  submit() {
    Object.assign(this.user, this.form.value);

    // transform literal objects back to instances of PartTimeSettings
    this.user.partTimeSettings = this.user.partTimeSettings.map((a) =>
      Object.assign(new PartTimeSetting(), { ...a, factor: a.factor / 100 }),
    );
    // If the factor for entryDate is the default case of 1, remove it
    if (this.user.partTimeSettings[0].factor === 1) {
      this.user.partTimeSettings.shift();
    }

    this.user.vacationDaysSettings = this.user.vacationDaysSettings.map((s) =>
      Object.assign(new VacationDaysSetting(), s),
    );

    if (this.user.vacationDaysSettings[0].vacationDays === 25) {
      this.user.vacationDaysSettings.shift();
    }

    this.userService.update(this.user).subscribe({
      next: () => this.cancel(),
      error: (err) => this.handleError(err),
    });
  }

  cancel() {
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['user']);
  }

  private handleError(errorResponse: ErrorResponse) {
    if (!errorResponse.tryToHandleWith(this.form)) {
      this.errorService.notifyGeneralError();
    }
  }
}
