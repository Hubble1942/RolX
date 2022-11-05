import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { Activity } from '@app/projects/core/activity';
import { filter } from 'rxjs';
import {
  StopToggleDialogAction,
  StopToggleDialogComponent,
  StopToggleDialogData,
} from './stop-toggle-dialog/stop-toggle-dialog.component';
import { Record } from '@app/records/core/record';
import { RecordEntry } from '@app/records/core/record-entry';
import { Duration } from '@app/core/util/duration';
import { WorkRecordService } from '@app/records/core/work-record.service';

@Component({
  selector: 'rolx-toggle',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.scss'],
})
export class ToggleComponent implements OnInit {
  @Input()
  activity!: Activity;

  @Input()
  records!: Record[];

  /*
  @Input()
  disabled!: boolean;
  */

  @Output()
  changed = new EventEmitter<Record>();

  // TODO fma: dieser Key muss pro activity eineindeutig sein. Activity ID oder so muss mit einbezogen werden.
  private readonly START_TIME_KEY = 'startTime';
  private readonly TOGGLE_ACTIVE = 'toggleActive';

  //private startTime?: Date;

  constructor(private readonly dialog: MatDialog, private workRecordService: WorkRecordService) {}

  ngOnInit(): void {
    // TODO fma: get start time from local storage
    //this.startTime = this.getStartTime();
  }

  startToggle(): void {
    this.storeStartTimeInLocalStorage(new Date());
  }

  startToggleDisabled(): boolean {
    return !!this.getStartTime() || !!this.getToggleActive();
  }

  stopToggle(): void {
    // TODO fma: pass the list of activities
    const data: StopToggleDialogData = {};

    this.dialog
      .open(StopToggleDialogComponent, {
        closeOnNavigation: true,
        data,
      })
      .afterClosed()
      .pipe(filter((r) => r != null))
      .subscribe((r) => {
        if (r.dialogAction === StopToggleDialogAction.MoreWork) {
          // Do nothing
        } else if (r.dialogAction === StopToggleDialogAction.DeleteTimer) {
          this.clearStartTimeInLocalStorage();
        } else if (r.dialogAction === StopToggleDialogAction.Store) {
          //this.openMultiEntriesDialog();

          let reocrdOfToday = this.records.find((r) => r.isToday);
          let startTime = this.getStartTime();
          if (reocrdOfToday && startTime) {
            let recordEntry = new RecordEntry();
            recordEntry.activityId = this.activity.id;
            recordEntry.comment = r.comment;

            let durationInHours =
              (new Date().getTime() - (startTime as Date).getTime()) / (1000 * 60 * 60);

            //let durationInHours = 2;
            recordEntry.duration = Duration.fromHours(durationInHours);
            reocrdOfToday.entries.push(recordEntry);
            this.changed.emit(reocrdOfToday);
            this.clearStartTimeInLocalStorage();
          }
        }
      });
  }

  /*
  submit(record: Record, index: number) {
    this.workRecordService.update(this.user.id, record).subscribe({
      next: (r) => (this.records[index] = r),
      error: (err) => {
        console.error(err);
        this.errorService.notifyGeneralError();
      },
    });
  }
  */

  /*
  openMultiEntriesDialog(): void {
    const data: MultiEntriesDialog2Data = {
      record: this.record,
      activity: this.activity,
    };

    this.dialog
      .open(MultiEntriesDialog2Component, {
        closeOnNavigation: true,
        data,
      })
      .afterClosed()
      .pipe(filter((r) => r != null))
      .subscribe((r) => {
        // TODO fma: store the data in backend
        this.clearStartTimeInLocalStorage();
      });
  }
  */

  stopToggleDisabled(): boolean {
    return !this.getStartTime();
  }

  private storeStartTimeInLocalStorage(startTime: Date): void {
    localStorage.setItem(this.getStartTimeKey(), startTime.toUTCString());
    localStorage.setItem(this.TOGGLE_ACTIVE, 'true');
  }

  private clearStartTimeInLocalStorage(): void {
    localStorage.removeItem(this.getStartTimeKey());
    localStorage.removeItem(this.TOGGLE_ACTIVE);
  }

  private getStartTime(): Date | undefined {
    let startTimeFromLocalStorage = localStorage.getItem(this.getStartTimeKey());
    if (startTimeFromLocalStorage !== null) {
      return new Date(startTimeFromLocalStorage);
    }

    return undefined;
  }

  private getToggleActive(): boolean {
    let toggleActive = localStorage.getItem(this.TOGGLE_ACTIVE);
    return toggleActive !== null;
  }

  private getStartTimeKey(): string {
    return this.START_TIME_KEY + this.activity.id;
  }
}
