import { Component, EventEmitter, Inject, Input, LOCALE_ID, OnChanges, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ErrorResponse } from '@app/core/error/error-response';
import { ErrorService } from '@app/core/error/error.service';
import { Duration } from '@app/core/util/duration';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Billability } from '@app/projects/core/billability';
import { BillabilityService } from '@app/projects/core/billability.service';
import { Subproject } from '@app/projects/core/subproject';

@Component({
  selector: 'rolx-activity-form',
  templateUrl: './activity-form.component.html',
  styleUrls: ['./activity-form.component.scss'],
})
export class ActivityFormComponent implements OnChanges {
  @Input() subproject!: Subproject;
  @Input() activity!: Activity;
  @Input() error?: ErrorResponse;
  @Output() cancel: EventEmitter<void> = new EventEmitter<void>();
  @Output() save: EventEmitter<{activity: Activity; addAnother: boolean}> =
    new EventEmitter<{activity: Activity; addAnother: boolean}>();

  form = this.fb.group({
    number: ['', [Validators.required, Validators.min(1), Validators.max(99)]],
    name: ['', Validators.required],
    startDate: ['', Validators.required],
    endDate: [null],
    budget: [null, Validators.min(0)],
    planned: [null, Validators.min(0)],
    billabilityId: [null, Validators.required],
  });

  billabilities: Billability[] = [];

  constructor(
    private readonly fb: FormBuilder,
    private readonly billabilityService: BillabilityService,
    private readonly errorService: ErrorService,
    @Inject(LOCALE_ID) private readonly locale: string,
  ) {
    this.billabilityService
      .getAll()
      .subscribe((billabilities) => (this.billabilities = billabilities));
  }

  ngOnChanges() {
    assertDefined(this, 'subproject');
    assertDefined(this, 'activity');

    if(this.error != null){
      // Force validators to show error message
      this.form.markAllAsTouched();
      this.handleError(this.error);
    } else {
      // clean form, validator and submitted state
      this.form.reset();
      this.form.markAsPristine();
    }
    this.form.patchValue(this.activity);
    this.formBudget = this.activity.budget;
    this.formPlanned = this.activity.planned;
  }

  get isNew() {
    return this.activity.id === 0;
  }

  get formBudget(): Duration {
    return this.getFormDuration('budget');
  }

  set formBudget(value: Duration) {
    this.setFormDuration('budget', value);
  }

  get formPlanned(): Duration {
    return this.getFormDuration('planned');
  }

  set formPlanned(value: Duration) {
    this.setFormDuration('planned', value);
  }

  hasError(controlName: string, errorName: string | string[]) {
    if (Array.isArray(errorName)) {
      return errorName.some((e) => this.form.controls[controlName].hasError(e));
    }

    return this.form.controls[controlName].hasError(errorName);
  }

  submit(addNew=false) {
    Object.assign(this.activity, this.form.value);
    this.activity.budget = this.formBudget;
    this.activity.planned = this.formPlanned;
    this.save.emit({activity: this.activity, addAnother: addNew});
  }

  private getFormDuration(durationName: string): Duration {
    const duration = Number.parseFloat(this.form.controls[durationName].value);
    return !Number.isNaN(duration) ? Duration.fromPersonDays(duration) : Duration.Zero;
  }

  private setFormDuration(durationName: string, value: Duration) {
    const formValue =
    value && !value.isZero
      ? value.personDays.toLocaleString(this.locale, {
          maximumFractionDigits: 1,
          useGrouping: false,
        })
      : null;
  this.form.controls[durationName].setValue(formValue);
  }

  private handleError(errorResponse: ErrorResponse) {
    if (!errorResponse.tryToHandleWith(this.form)) {
      this.errorService.notifyGeneralError();
    }
  }
}
