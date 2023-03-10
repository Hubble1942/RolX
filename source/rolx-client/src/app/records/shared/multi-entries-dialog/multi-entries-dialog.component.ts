import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, ValidationErrors } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Duration } from '@app/core/util/duration';
import { StrongTypedFormControl } from '@app/core/util/strong-typed-form-control';
import { TimeOfDay } from '@app/core/util/time-of-day';
import { Activity } from '@app/projects/core/activity';
import { ActivityService } from '@app/projects/core/activity.service';
import { EditLockService } from '@app/records/core/edit-lock.service';
import { Record } from '@app/records/core/record';
import { RecordEntry } from '@app/records/core/record-entry';
import { ParentErrorStateMatcher } from '@app/records/shared/multi-entries-dialog/parent-error-state-matcher';

import { FormRow } from './form-row';

export interface MultiEntriesDialogData {
  record: Record;
  activity: Activity;
}

@Component({
  selector: 'rolx-multi-entries-dialog',
  templateUrl: './multi-entries-dialog.component.html',
  styleUrls: ['./multi-entries-dialog.component.scss'],
})
export class MultiEntriesDialogComponent implements OnInit {
  private allActivities: Activity[] = [];

  @ViewChild('scrollable') private scrollable?: ElementRef;
  readonly isLocked = this.editLockService.isLocked(this.data.record.date);
  readonly errorStateMatcher = new ParentErrorStateMatcher();

  form = this.fb.group({
    entries: this.fb.array([], () => this.validateOverlappingEntries()),
  });

  formRows: FormRow[] = [];
  displayedColumns: string[] = ['mode', 'begin', 'end', 'pause', 'duration', 'comment', 'tools'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: MultiEntriesDialogData,
    private dialogRef: MatDialogRef<MultiEntriesDialogComponent>,
    private fb: FormBuilder,
    private readonly editLockService: EditLockService,
    private activityService: ActivityService,
  ) {
    this.dialogRef.disableClose = true;
  }

  ngOnInit() {
    this.activityService
      .getAll(this.data.record.date)
      .subscribe((activities) => (this.allActivities = activities));
    const entries = this.data.record.entriesOf(this.data.activity);

    if (entries.length) {
      entries.filter((e) => !e.duration.isZero).forEach((e) => this.addRow(e));
    } else {
      this.addRow();
    }

    if (this.isLocked) {
      this.form.disable();
    }
  }

  get totalDuration(): Duration {
    return new Duration(
      this.formRows
        .filter((r) => r.duration.value)
        .reduce((sum, r) => sum + r.duration.value.seconds, 0),
    );
  }

  submit() {
    const entries = this.formRows.filter((r) => r.hasDuration).map((r) => r.toEntry());

    const record = this.data.record.replaceEntriesOfActivity(this.data.activity, entries);
    this.dialogRef.close(record);
  }

  close() {
    this.dialogRef.close(null);
  }

  tryAddRow(index: number) {
    if (index === this.formRows.length - 1) {
      this.addRow();
    }
  }

  addRow(entry?: RecordEntry | null) {
    const row =
      entry != null
        ? new FormRow(entry)
        : new FormRow(this.formRows[this.formRows.length - 1]?.isBeginEndBased ?? true);

    (this.form.controls['entries'] as FormArray).push(row.group);
    this.formRows = [...this.formRows, row];
    setTimeout(() => this.scrollToBottom());
  }

  removeRow(row: FormRow): void {
    this.formRows = this.formRows.filter((formRow) => formRow !== row);

    const entriesControl = this.form.controls['entries'] as FormArray;
    entriesControl.removeAt(entriesControl.controls.indexOf(row.group));

    if (this.formRows.length === 0) {
      this.addRow();
    }
  }

  getActivityOverlappingTooltipTextFor(control: FormControl): string {
    const overlappingEntries = control.errors?.['overlapsOtherActivity'] as RecordEntry[] | null;
    if (overlappingEntries == null) {
      return '';
    }

    const asText = overlappingEntries.map((e) => {
      const activityName =
        this.allActivities.find((a) => a.id === e.activityId)?.fullName ?? `<id: ${e.activityId}>`;
      return `Zeit: ${e.begin?.toString()} - ${e.end?.toString()}\nAktivität: ${activityName}`;
    });

    return (
      'Dieser Eintrag überlappt mit einem Eintrag einer anderen Aktivität.\n\n' +
      asText.join('\n\n')
    );
  }

  isOverlapping(control: FormControl): boolean {
    return (
      control.errors?.['localOverlapping'] != null ||
      control.errors?.['overlapsOtherActivity'] != null
    );
  }

  private cleanOverlappingErrors(control: FormControl) {
    if (control.errors != null) {
      delete control.errors['localOverlapping'];
      delete control.errors['overlapsOtherActivity'];
      if (Object.keys(control.errors).length === 0) {
        control.setErrors(null);
      }
    }
  }

  private checkAndSetOverlappingError(
    target: TimeOfDay,
    entryTarget: TimeOfDay,
    intervalBegin: TimeOfDay,
    intervalEnd: TimeOfDay,
    control: FormControl,
    entry: RecordEntry & { begin: TimeOfDay; end: TimeOfDay },
  ) {
    if (
      target.isInInterval(entry.begin, entry.end) ||
      target.sub(entryTarget).isZero ||
      (entry.begin.isInInterval(intervalBegin, intervalEnd) &&
        entry.end.isInInterval(intervalBegin, intervalEnd))
    ) {
      control.markAsDirty();
      if (this.data.activity.id === entry.activityId || entry.activityId == null) {
        control.setErrors({ localOverlapping: entry });
      } else {
        const otherOverlaps = control.errors?.['overlapsOtherActivity'];
        if (otherOverlaps != null) {
          otherOverlaps.push(entry);
        } else {
          control.setErrors({ overlapsOtherActivity: [entry] });
        }
      }
    }
  }

  private validateOverlappingEntries(): ValidationErrors | null {
    if (this.formRows == null) {
      return null;
    }
    const otherActivitiesEntries = this.data.record.entries.filter(
      (e) => e.isBeginEndBased && e.activityId !== this.data.activity.id,
    );
    this.formRows.forEach((row) => {
      this.cleanOverlappingErrors(row.begin);
      this.cleanOverlappingErrors(row.end);
      if (!this.isRowValid(row) || row.begin.errors != null || row.end.errors != null) {
        return;
      }
      const begin = row.begin.typedValue;
      const end = row.end.typedValue;
      const others = this.formRows
        .filter((other) => other !== row && this.isRowValid(other))
        .map((other) => other.toEntry());
      const allEntries = others
        .concat(otherActivitiesEntries)
        .filter(
          (e): e is RecordEntry & { begin: TimeOfDay; end: TimeOfDay } =>
            e.isBeginEndBased && e.begin != null,
        );
      allEntries.forEach((entry) => {
        this.checkAndSetOverlappingError(begin, entry.begin, begin, end, row.begin, entry);
        this.checkAndSetOverlappingError(end, entry.end, begin, end, row.end, entry);
      });
    });
    return null;
  }

  private isRowValid(row: FormRow): row is FormRow & {
    readonly begin: StrongTypedFormControl<TimeOfDay> & { readonly typedValue: TimeOfDay };
    readonly end: StrongTypedFormControl<TimeOfDay> & { readonly typedValue: TimeOfDay };
    readonly group: FormGroup & { readonly errors: null };
  } {
    return (
      row.beginEndBased &&
      row.begin.typedValue != null &&
      row.end.typedValue != null &&
      row.group.errors == null
    );
  }

  private scrollToBottom() {
    try {
      if (this.scrollable) {
        this.scrollable.nativeElement.scroll({
          top: this.scrollable.nativeElement.scrollHeight,
          behavior: 'smooth',
        });
      }
    } catch (err) {}
  }
}
