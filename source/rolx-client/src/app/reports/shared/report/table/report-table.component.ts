import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { SortService } from '@app/core/persistence/sort.service';
import { assertDefined } from '@app/core/util/utils';
import { SubprojectFilterService } from '@app/projects/core/subproject-filter.service';
import { Report } from '@app/reports/core/report';
import { ReportEntry } from '@app/reports/core/report-entry';

@Component({
  selector: 'rolx-report-table',
  templateUrl: './report-table.component.html',
  styleUrls: ['./report-table.component.scss'],
})
export class ReportTableComponent implements OnInit {
  private _report!: Report;
  readonly dataSource = new MatTableDataSource<ReportEntry>();

  displayedColumns: string[] = [];
  @ViewChild(MatSort, { static: true }) sort!: MatSort;

  @Input()
  get report(): Report {
    return this._report;
  }
  set report(value: Report) {
    this._report = value;
    this.dataSource.data = value.reportEntries;
    this.displayedColumns = Array.from(this.getDisplayedColumns());
  }

  constructor(
    private readonly sortService: SortService,
    public readonly filterService: SubprojectFilterService,
  ) {}

  ngOnInit(): void {
    assertDefined(this, 'sort');

    this.dataSource.sort = this.sort;
    this.dataSource.filter = this.filterService.filterText.toLowerCase();

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

  applyFilter(value: string) {
    this.filterService.filterText = value;
    this.dataSource.filter = this.filterService.filterText.toLowerCase();
  }

  private *getDisplayedColumns() {
    yield 'date';
    // yield 'projectNumber';
    // yield 'customerName';
    // yield 'projectName';
    // yield 'subprojectNumber';
    // yield 'subprojectName';
    yield 'activityNumber';
    yield 'activityName';
    yield 'billabilityName';
    yield 'userName';
    yield 'duration';
    yield 'comment';
  }
}
