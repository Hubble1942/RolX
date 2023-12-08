import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { assertDefined } from '@app/core/util/utils';
import { VacationDaysSetting } from '@app/users/core/vacation-days-setting';
import { Moment } from 'moment';

@Component({
  selector: 'rolx-vacation-days-settings-form',
  templateUrl: './vacation-days-settings-form.component.html',
  styleUrls: ['./vacation-days-settings-form.component.scss'],
})
export class VacationDaysSettingsFormComponent implements OnInit {
  readonly displayedColumns = ['startDate', 'vacationDays', 'tools'];

  @Input()
  formArray!: FormArray;

  @Input()
  initialVacationDaysSettings!: VacationDaysSetting[];

  @Input()
  entryDate: Moment | null = null;

  @Input()
  leaveDate: Moment | null = null;

  get formArrayAsArray(): FormGroup[] {
    // This conversion is necessary for material table to render the form array
    // Somehow FormArray.controls does not behave the same way as a normal array
    return Array.from(this.formArray.controls as FormGroup[]);
  }

  ngOnInit(): void {
    assertDefined(this, 'formArray');
    assertDefined(this, 'initialVacationDaysSettings');

    this.formArray.setValidators((c) => this.validateVacationDaysDate(c as FormArray));
    this.initialVacationDaysSettings.forEach((s) => this.addVacationDaysRow(s));
  }

  addVacationDaysRow(entry?: VacationDaysSetting) {
    const startDate = new FormControl(entry?.startDate, [Validators.required]);
    const vacationDays = new FormControl(entry?.vacationDays, [
      Validators.required,
      Validators.min(20),
      Validators.max(365),
    ]);
    const group = new FormGroup({
      startDate,
      vacationDays,
    });
    this.formArray.push(group);
  }

  deleteVacationDaysRow(index: number) {
    this.formArray.removeAt(index);
  }

  private validateVacationDaysDate(control: FormArray): ValidationErrors | null {
    let arrayContainsErrors = false;
    control.controls.forEach((c1) => {
      const hasError =
        c1.value.startDate != null &&
        ((this.entryDate != null && this.entryDate.isAfter(c1.value.startDate)) ||
          (this.leaveDate != null && this.leaveDate.isBefore(c1.value.startDate)) ||
          control.controls.some(
            (c2) =>
              c1 !== c2 &&
              c2.value.startDate != null &&
              (c1.value.startDate as Moment).isSame(c2.value.startDate, 'day'),
          ));
      arrayContainsErrors ||= hasError;
      // Set errors manually so they get reset if another conflicting form resolves the error
      if (hasError) {
        (c1 as FormGroup).controls['startDate'].setErrors({ hasError: true });
      }
    });
    return arrayContainsErrors ? { containsErrors: true } : null;
  }
}
