import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ToggleService } from '@app/core/persistence/toggle.service';
import { Duration } from '@app/core/util/duration';
import { TimeOfDay } from '@app/core/util/time-of-day';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Record } from '@app/records/core/record';
import { RecordEntry } from '@app/records/core/record-entry';
import {
  MultiEntriesDialogComponent,
  MultiEntriesDialogData,
} from '@app/records/shared/multi-entries-dialog/multi-entries-dialog.component';
import * as moment from 'moment';
import { filter } from 'rxjs';

@Component({
  selector: 'rolx-stop-toggle',
  templateUrl: './stop-toggle.component.html',
})
export class StopToggleComponent implements OnInit {
  @Input()
  activity!: Activity;

  @Input()
  record!: Record;

  @Output()
  changed = new EventEmitter<Record>();

  constructor(private toggleService: ToggleService, private dialog: MatDialog) {}

  get toggleInactive(): boolean {
    return !this.toggleService.active;
  }

  ngOnInit(): void {
    assertDefined(this, 'activity');
    assertDefined(this, 'record');
  }

  stopToggle() {
    const newEntry = this.createEntryFromToggle();
    if (!newEntry) {
      return;
    }

    const entries = this.record.entriesOf(this.activity).concat([newEntry]);
    const recordCopy = this.record.replaceEntriesOfActivity(this.activity, entries);

    const data: MultiEntriesDialogData = {
      activity: this.activity,
      record: recordCopy,
    };

    this.dialog
      .open(MultiEntriesDialogComponent, {
        closeOnNavigation: true,
        data,
      })
      .afterClosed()
      .pipe(filter((r) => r != null))
      .subscribe((r) => {
        this.changed.emit(r);
        this.toggleService.clearToggle();
      });
  }

  private createEntryFromToggle(): RecordEntry | undefined {
    const startTime = this.toggleService.startTime;
    if (!startTime) {
      return undefined;
    }

    const entry = new RecordEntry();
    entry.activityId = this.activity.id;

    const now = moment();
    // Just use duration when worked through midnight
    if (startTime.date() === now.date()) {
      entry.begin = TimeOfDay.fromMoment(startTime);
    }
    entry.duration = Duration.fromMoments(startTime, moment());

    return entry;
  }
}
