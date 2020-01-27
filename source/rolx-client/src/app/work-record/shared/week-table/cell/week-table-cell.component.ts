import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatButton, MatDialog } from '@angular/material';
import { Phase } from '@app/account/core';
import { GridCoordinates, GridNavigationService } from '@app/core/grid-navigation';
import { Duration } from '@app/core/util';
import { Record, RecordEntry, WorkRecordService } from '@app/work-record/core';
import { EntriesEditComponent, MultiEntriesDialogComponent, MultiEntriesDialogData } from '@app/work-record/shared';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'rolx-week-table-cell',
  templateUrl: './week-table-cell.component.html',
  styleUrls: ['./week-table-cell.component.scss'],
})
export class WeekTableCellComponent implements OnInit, OnDestroy {

  private readonly subscription = new Subscription();
  private coordinates = new GridCoordinates(undefined, undefined);

  @ViewChild(EntriesEditComponent, {static: false})
  private entriesEdit: EntriesEditComponent;

  @ViewChild('moreButton', {static: false})
  private moreButton: MatButton;

  @Input()
  record: Record;

  @Input()
  phase: Phase;

  @Input()
  get row(): number {
    return this.coordinates.row;
  }
  set row(value: number) {
    this.coordinates = new GridCoordinates(this.coordinates.column, value);
  }

  @Input()
  get column(): number {
    return this.coordinates.column;
  }
  set column(value: number) {
    this.coordinates = new GridCoordinates(value, this.coordinates.row);
  }

  get entries(): RecordEntry[] {
    return this.record.entriesOf(this.phase);
  }

  get isSimpleEditable(): boolean {
    return this.entries.length <= 1
        && this.entries.every(e => e.isDurationOnly)
        && this.isPhaseOpen;
  }

  get isPhaseOpen(): boolean {
    return this.phase.isOpenAt(this.record.date);
  }

  get totalDuration(): Duration {
    return new Duration(
      this.entries.reduce(
        (sum, e) => sum + e.duration.seconds, 0));
  }

  constructor(private gridNavigationService: GridNavigationService,
              private dialog: MatDialog,
              private workRecordService: WorkRecordService) { }

  ngOnInit() {
    this.subscription.add(
      this.gridNavigationService.coordinates$
        .pipe(
          filter(c => this.coordinates.isSame(c)),
        )
        .subscribe(c => this.enter(c)));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  up() {
    this.gridNavigationService.navigateTo(this.coordinates.up());
  }

  down() {
    this.gridNavigationService.navigateTo(this.coordinates.down());
  }

  submitSingleEntry(entry: RecordEntry) {
    const record = this.record.replaceEntriesOfPhase(this.phase, [entry]);
    this.workRecordService.update(record)
      .subscribe(() => this.record.entries = record.entries);
  }

  editEntries() {
    const data: MultiEntriesDialogData = {
      record: this.record,
      phase: this.phase,
    };

    this.dialog.open(MultiEntriesDialogComponent, {
      closeOnNavigation: true,
      data,
    });
  }

  private enter(coordinates: GridCoordinates) {
    if (this.entriesEdit) {
      this.entriesEdit.enter();
    } else if (this.moreButton) {
      this.moreButton.focus();
    } else {
      this.gridNavigationService.navigateTo(coordinates.down());
    }
  }

}