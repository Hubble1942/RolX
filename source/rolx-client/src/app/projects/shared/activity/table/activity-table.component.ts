import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthService } from '@app/auth/core/auth.service';
import { SortService } from '@app/core/persistence/sort.service';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Subproject } from '@app/projects/core/subproject';

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

  get hasWriteAccess() {
    return this.authService.currentApprovalOrError.isSupervisor;
  }

  @Input()
  get subproject(): Subproject {
    return this._subproject;
  }
  set subproject(value: Subproject) {
    this._subproject = value;
    this.dataSource.data = value.activities;
    this.displayedColumns = Array.from(this.getDisplayedColumns());
  }

  constructor(
    private readonly authService: AuthService,
    private readonly sortService: SortService,
  ) {}

  ngOnInit(): void {
    assertDefined(this, '_subproject');
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

  tpd(activity: Activity): Activity {
    return activity;
  }

  private *getDisplayedColumns() {
    yield 'number';
    yield 'name';
    yield 'startDate';
    yield 'endDate';
    yield 'budgetTime';

    if (!this.subproject.planned.isZero) {
      yield 'plannedTime';
    }

    yield 'actualTime';
    yield 'isBillable';

    if (this.hasWriteAccess) {
      yield 'tools';
    }
  }
}
