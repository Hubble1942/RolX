import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MultiEntriesDialogData } from '../../multi-entries-dialog/multi-entries-dialog.component';

// TODO fma: what data is required?
export interface StopToggleDialogData {}
export interface StopToggleDialogResponseData {
  dialogAction: StopToggleDialogAction;
  comment: string;
}

export enum StopToggleDialogAction {
  MoreWork,
  DeleteTimer,
  Store,
}

@Component({
  selector: 'rolx-stop-toggle-dialog',
  templateUrl: './stop-toggle-dialog.component.html',
  styleUrls: ['./stop-toggle-dialog.component.scss'],
})
export class StopToggleDialogComponent implements OnInit {
  form = this.fb.group({});

  comment!: string;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: StopToggleDialogData,
    private dialogRef: MatDialogRef<StopToggleDialogComponent>,
    private fb: FormBuilder,
  ) {}

  ngOnInit(): void {}

  submit() {
    // TODO fma: what needs to be done on submit?
    /*
    const entries = this.formRows.filter((r) => r.hasDuration).map((r) => r.toEntry());

    const record = this.data.record.replaceEntriesOfActivity(this.data.activity, entries);
    */
    // TODO fma: what needs to be returned?
    this.dialogRef.close(this.getStoreResponseData());
  }

  close() {
    this.dialogRef.close(this.getMoreWorkResponseData());
  }

  getMoreWorkResponseData(): StopToggleDialogResponseData {
    return {
      dialogAction: StopToggleDialogAction.MoreWork,
      comment: '',
    };
  }

  getDeleteTimerResponseData(): StopToggleDialogResponseData {
    return {
      dialogAction: StopToggleDialogAction.DeleteTimer,
      comment: '',
    };
  }

  getStoreResponseData(): StopToggleDialogResponseData {
    return {
      dialogAction: StopToggleDialogAction.Store,
      comment: this.comment,
    };
  }
}
