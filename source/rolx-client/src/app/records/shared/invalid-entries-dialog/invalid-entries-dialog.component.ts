import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Duration } from '@app/core/util/duration';
import { Activity } from '@app/projects/core/activity';
import { Record } from '@app/records/core/record';

export interface InvalidEntriesDialogData {
  record: Record;
  offendingActivities: Activity[];
}

@Component({
  selector: 'rolx-invalid-entries-dialog',
  templateUrl: './invalid-entries-dialog.component.html',
  styleUrls: ['./invalid-entries-dialog.component.scss'],
})
export class InvalidEntriesDialogComponent {
  displayedColumns: string[] = ['activity', 'closed', 'duration'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: InvalidEntriesDialogData,
    private dialogRef: MatDialogRef<InvalidEntriesDialogData>,
  ) {
    this.dialogRef.disableClose = true;
  }

  deleteEntries() {
    const record = this.data.offendingActivities.reduce(
      (r, a) => r.replaceEntriesOfActivity(a, []),
      this.data.record,
    );
    this.dialogRef.close(record);
  }

  durationFor(activity: Activity) {
    return new Duration(
      this.data.record.entriesOf(activity).reduce((sum, e) => sum + e.duration.seconds, 0),
    );
  }
}
