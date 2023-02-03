import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ReportRange } from '@app/reports/core/report-range';
import { ReportRangeType } from '@app/reports/core/report-range-type';
import { ReportService } from '@app/reports/core/report.service';
import * as moment from 'moment';
import { BehaviorSubject, combineLatest, Observable, Subscription } from 'rxjs';

@Component({
  selector: 'rolx-report-range',
  templateUrl: './report-range.component.html',
  styleUrls: ['./report-range.component.scss'],
})
export class ReportRangeComponent implements OnInit, OnDestroy {
  private subscription = new Subscription();

  // used to update the currentRangeType after a new value is received from the range input
  private rangeTypeFromComponentInput$ = new BehaviorSubject<string | undefined>(undefined);

  // used to break update loops
  private lastRangeFromInput?: ReportRange;

  @Input() subprojectStartDate: moment.Moment | null = null;

  @Output() rangeChange = new EventEmitter<ReportRange | undefined>();

  currentRangeType?: ReportRangeType;

  rangeTypes$ = this.reportService.reportRangeTypes$;

  typeControl = new FormControl(undefined, [Validators.required]);
  startControl = new FormControl();
  endControl = new FormControl();

  readonly endMonthControl = new FormControl(moment(), [
    Validators.required,
    (control) =>
      this.subprojectStartDate?.isSameOrBefore(control.value, 'month') ?? true
        ? null
        : { monthBeforeStart: true },
  ]);

  formGroup = new FormGroup({
    type: this.typeControl,
    start: this.startControl,
    end: this.endControl,
    endMonth: this.endMonthControl,
  });

  constructor(private readonly reportService: ReportService) {}

  ngOnInit(): void {
    this.subscription.add(
      combineLatest([
        this.formGroup.valueChanges as Observable<{
          type?: string | null;
          start: moment.Moment | null;
          end: moment.Moment | null;
          endMonth: moment.Moment | null;
        }>,
        this.rangeTypes$,
      ]).subscribe({
        next: ([values, rangeTypes]) => {
          if (values.type === undefined || values.type === null) {
            this.currentRangeType = undefined;
            return;
          }
          const rangeType = rangeTypes.find((x) => x.id === values.type);
          this.currentRangeType = rangeType;
          if (rangeType == null) {
            return;
          }

          if (rangeType.hasCustomEndMonth) {
            if (
              values.endMonth !== null &&
              (this.subprojectStartDate === null || values.endMonth > this.subprojectStartDate)
            ) {
              this.rangeChange.next(new ReportRange(values.type, undefined, values.endMonth));
            }
          } else if (rangeType.hasCustomStart && rangeType.hasCustomEnd) {
            if (values.start !== null && values.end !== null && values.start < values.end) {
              this.rangeChange.next(new ReportRange(values.type, values.start, values.end));
            }
          } else {
            this.rangeChange.next(new ReportRange(values.type));
          }
        },
      }),
    );

    this.subscription.add(
      combineLatest([this.rangeTypeFromComponentInput$, this.rangeTypes$]).subscribe({
        next: ([type, rangeTypes]) => {
          if (type === undefined) {
            this.currentRangeType = undefined;
          } else {
            this.currentRangeType = rangeTypes.find((x) => x.id === type);
          }
        },
      }),
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  @Input() set range(value: ReportRange | undefined) {
    if (ReportRange.equals(value, this.lastRangeFromInput)) {
      return;
    }
    this.lastRangeFromInput = value;

    if (value === undefined) {
      this.formGroup.reset(null, { emitEvent: false });
      if (this.subprojectStartDate !== null) {
        this.startControl.setValue(this.subprojectStartDate, { emitEvent: false });
      }
    } else {
      this.typeControl.setValue(value.reportRangeType, { emitEvent: false });
      this.startControl.setValue(value.customStart, { emitEvent: false });
      this.endControl.setValue(value.customEnd, { emitEvent: false });
      this.endMonthControl.setValue(value.customEnd, { emitEvent: false });
      this.rangeTypeFromComponentInput$.next(value.reportRangeType);
    }
  }

  endMonthYearSelected(normalizedYear: moment.Moment) {
    const ctrlValue = this.endMonthControl.value ?? moment().month(0);
    ctrlValue.year(normalizedYear.year());
    this.endMonthControl.setValue(ctrlValue);
  }

  endMonthMonthSelected(normalizedMonth: moment.Moment, monthPicker: any) {
    const ctrlValue = this.endMonthControl.value;
    ctrlValue.month(normalizedMonth.month());
    this.endMonthControl.setValue(ctrlValue);
    monthPicker.close();
  }
}
