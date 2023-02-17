import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { assertDefined } from '@app/core/util/utils';
import { PartTimeSetting } from '@app/users/core/part-time-setting';
import { Moment } from 'moment';

@Component({
  selector: 'rolx-parttime-form',
  templateUrl: './parttime-form.component.html',
  styleUrls: ['./parttime-form.component.scss'],
})
export class PartTimeSettingsFormComponent implements OnInit {
  readonly displayedColumns = ['startDate', 'factor', 'tools'];

  @Input()
  formArray!: FormArray;

  @Input()
  initialPartTimeSettings!: PartTimeSetting[];

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
    assertDefined(this, 'initialPartTimeSettings');

    this.formArray.setValidators((c) => this.validatePartTimeDate(c as FormArray));
    this.initialPartTimeSettings.forEach((s) => this.addPartTimeRow(s));
  }

  addPartTimeRow(entry?: PartTimeSetting) {
    const startDate = new FormControl(entry?.startDate, [Validators.required]);
    const factor = new FormControl(entry?.factor, [
      Validators.required,
      Validators.min(1),
      Validators.max(100),
    ]);
    const group = new FormGroup({
      startDate,
      factor,
    });
    this.formArray.push(group);
  }

  deletePartTimeRow(index: number) {
    this.formArray.removeAt(index);
  }

  private validatePartTimeDate(control: FormArray): ValidationErrors | null {
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
      (c1 as FormGroup).controls['startDate'].setErrors(hasError ? { hasError: true } : null);
    });
    return arrayContainsErrors ? { containsErrors: true } : null;
  }
}
