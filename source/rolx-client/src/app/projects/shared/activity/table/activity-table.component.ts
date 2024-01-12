import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthService } from '@app/auth/core/auth.service';
import { Flag, FlagService } from '@app/core/persistence/flag-service';
import { SortService } from '@app/core/persistence/sort.service';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Subproject } from '@app/projects/core/subproject';
import * as moment from 'moment';

@Component({
  selector: 'rolx-activity-table',
  templateUrl: './activity-table.component.html',
  styleUrls: ['./activity-table.component.scss'],
})
export class ActivityTableComponent implements OnInit {
  private _subproject!: Subproject;

  readonly dataSource = new MatTableDataSource<Activity>();

  displayedColumns: string[] = [];
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  columns: { id: string; label: string; notHideable?: boolean; isHidden?: () => boolean }[] = [
    { id: 'number', label: '', notHideable: true },
    { id: 'name', label: 'Aktivität', notHideable: true },
    { id: 'startDate', label: 'Start' },
    { id: 'endDate', label: 'Ende' },
    { id: 'budgetTime', label: 'Soll' },
    { id: 'plannedTime', label: 'Geplant', isHidden: () => this.subproject.planned.isZero },
    { id: 'actualTime', label: 'Ist' },
    { id: 'isBillable', label: 'Verrechenbar' },
    { id: 'budgetConsumed', label: 'Status' },
    {
      id: 'plannedConsumed',
      label: 'Fortschritt Geplant',
      isHidden: () => this.subproject.planned.isZero,
    },
    { id: 'tools', label: '', notHideable: true },
  ];

  get hasWriteAccess() {
    return this.authService.currentApprovalOrError.isSupervisor;
  }

  get now() {
    return moment();
  }

  @Input()
  get subproject(): Subproject {
    return this._subproject;
  }

  set subproject(value: Subproject) {
    this._subproject = value;
    this.dataSource.data = value.activities;
    this.displayedColumns = this.getDisplayedColumns();
  }

  constructor(
    private readonly authService: AuthService,
    private readonly sortService: SortService,
    private readonly flagService: FlagService,
  ) {}

  ngOnInit(): void {
    assertDefined(this, 'subproject');
    assertDefined(this, 'sort');

    this.dataSource.sort = this.sort;

    this.sort.sort(
      this.sortService.get('Activity', {
        id: this.displayedColumns[0],
        start: 'asc',
        disableClear: true,
      }),
    );
    this.sort.sortChange.subscribe((sort) => this.sortService.set('Activity', sort));
  }

  updateColumns() {
    this.displayedColumns = Array.from(this.getDisplayedColumns());
  }

  tpd(activity: Activity): Activity {
    return activity;
  }

  columnLabel(id: string) {
    return this.columns.find((x) => x.id === id)?.label;
  }

  checkBoxForFlag(column: string): string {
    return this.flagService.get(this.columnFlag(column)) ? 'check_box' : 'check_box_outline_blank';
  }

  toggleFlag(column: string) {
    const flag = this.columnFlag(column);
    this.flagService.set(flag, !this.flagService.get(flag));
    this.updateColumns();
  }

  get menuColumns() {
    return this.columns.filter((x) => x.notHideable !== true);
  }

  private columnFlag(column: string) {
    return ('showColumn_' + column) as Flag;
  }

  private getDisplayedColumns() {
    return this.columns
      .filter((x) => {
        if (x.notHideable === true) {
          return true;
        }
        if (x.isHidden !== undefined && x.isHidden()) {
          return false;
        }
        return this.flagService.get(this.columnFlag(x.id));
      })
      .map((x) => x.id);
  }
}
