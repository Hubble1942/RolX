import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Duration } from '@app/core/util/duration';
import { DurationValidators } from '@app/core/util/duration.validators';
import { TimeFormControl } from '@app/core/util/time-form-control';
import { assertDefined } from '@app/core/util/utils';
import { BalanceCorrection } from '@app/users/core/balance-correction';
import { Moment } from 'moment';

@Component({
  selector: 'rolx-balance-corrections-form',
  templateUrl: './balance-corrections-form.component.html',
  styleUrls: ['./balance-corrections-form.component.scss'],
})
export class BalanceCorrectionsFormComponent implements OnInit {
  readonly displayedColumns = ['date', 'overtime', 'vacation', 'tools'];

  @Input()
  formArray!: FormArray;

  @Input()
  initialBalanceCorrection!: BalanceCorrection[];

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
    assertDefined(this, 'initialBalanceCorrection');

    this.formArray.setValidators((c) => this.validateDate(c as FormArray));
    this.initialBalanceCorrection.forEach((s) => this.addRow(s));
  }

  addRow(entry?: BalanceCorrection) {
    const date = new FormControl(entry?.date, [Validators.required]);
    const overtime = TimeFormControl.createDuration(entry?.overtime, [
      Validators.required,
      DurationValidators.min(Duration.fromHours(-1000000)),
      DurationValidators.max(Duration.fromHours(1000000)),
    ]);
    const vacation = TimeFormControl.createDuration(entry?.vacation, [
      Validators.required,
      DurationValidators.min(Duration.fromHours(-1000000)),
      DurationValidators.max(Duration.fromHours(1000000)),
    ]);
    const group = new FormGroup({
      date,
      overtime,
      vacation,
    });
    this.formArray.push(group);
  }

  deleteRow(index: number) {
    this.formArray.removeAt(index);
  }

  private validateDate(control: FormArray): ValidationErrors | null {
    let arrayContainsErrors = false;
    control.controls.forEach((c1) => {
      const hasError =
        c1.value.date != null &&
        ((this.entryDate != null && this.entryDate.isAfter(c1.value.date)) ||
          (this.leaveDate != null && this.leaveDate.isBefore(c1.value.date)) ||
          control.controls.some(
            (c2) =>
              c1 !== c2 &&
              c2.value.date != null &&
              (c1.value.date as Moment).isSame(c2.value.date, 'day'),
          ));
      arrayContainsErrors ||= hasError;
      // Set errors manually so they get reset if another conflicting form resolves the error
      if (hasError) {
        (c1 as FormGroup).controls['date'].setErrors({ invalidDate: true });
      }
    });
    return arrayContainsErrors ? { containsErrors: true } : null;
  }
}
