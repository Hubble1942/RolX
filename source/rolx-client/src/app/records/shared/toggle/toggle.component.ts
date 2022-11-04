import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { filter } from 'rxjs';
import {
  StopToggleDialogAction,
  StopToggleDialogComponent,
  StopToggleDialogData,
} from './stop-toggle-dialog/stop-toggle-dialog.component';

@Component({
  selector: 'rolx-toggle',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.scss'],
})
export class ToggleComponent implements OnInit {
  // TODO fma: dieser Key muss pro activity eineindeutig sein. Activity ID oder so muss mit einbezogen werden.
  private readonly START_TIME_KEY = 'startTime';

  //private startTime?: Date;

  constructor(private readonly dialog: MatDialog) {}

  ngOnInit(): void {
    // TODO fma: get start time from local storage
    //this.startTime = this.getStartTime();
  }

  startToggle(): void {
    this.storeStartTimeInLocalStorage(new Date());
  }

  startToggleDisabled(): boolean {
    return !!this.getStartTime();
  }

  storeStartTimeInLocalStorage(startTime: Date): void {
    localStorage.setItem(this.START_TIME_KEY, startTime.toUTCString());
  }

  clearStartTimeInLocalStorage(): void {
    localStorage.removeItem(this.START_TIME_KEY);
  }

  getStartTime(): Date | undefined {
    let startTimeFromLocalStorage = localStorage.getItem(this.START_TIME_KEY);
    if (startTimeFromLocalStorage !== null) {
      return new Date(startTimeFromLocalStorage);
    }

    return undefined;
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
        }
      });
  }

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
}
