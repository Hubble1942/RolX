import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AuthService } from '@app/auth/core/auth.service';
import { SortService } from '@app/core/persistence/sort.service';
import { assertDefined } from '@app/core/util/utils';
import { ReportEntry } from '@app/reports/core/report-entry';

@Component({
  selector: 'rolx-report-table',
  templateUrl: './report-table.component.html',
  styleUrls: ['./report-table.component.scss'],
})
export class ReportTableComponent implements OnInit {
  private _reportEntries!: ReportEntry[];
  readonly dataSource = new MatTableDataSource<ReportEntry>();

  @Input()
  get reportEntries(): ReportEntry[] {
    return this._reportEntries;
  }
  set reportEntries(value: ReportEntry[]) {
    this._reportEntries = value;
    this.dataSource.data = value;
  }

  displayedColumns: string[] = [
      'userName',
      'duration',
      'comment',
    ];

  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  constructor(
    private readonly authService: AuthService,
    private readonly sortService: SortService,
  ) {}

  ngOnInit(): void {
    assertDefined(this, '_reportEntries');
    assertDefined(this, 'sort');

    this.dataSource.sort = this.sort;

    this.sort.sort(
      this.sortService.get('ReportEntry', {
        id: this.displayedColumns[0],
        start: 'asc',
        disableClear: true,
      }),
    );
    this.sort.sortChange.subscribe((sort) => this.sortService.set('ReportEntry', sort));
  }
  tpd(reportEntry: ReportEntry): ReportEntry {
    return reportEntry;
  }
}
