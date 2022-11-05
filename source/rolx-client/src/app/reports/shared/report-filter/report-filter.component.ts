import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { SubprojectShallow } from '@app/projects/core/subproject-shallow';
import { SubprojectService } from '@app/projects/core/subproject.service';
import { DateRange } from '@app/reports/core/date-range';
import { ReportFilter } from '@app/reports/core/report-filter';
import { User } from '@app/users/core/user';
import { UserService } from '@app/users/core/user.service';
import * as moment from 'moment';

@Component({
  selector: 'rolx-report-filter',
  templateUrl: './report-filter.component.html',
  styleUrls: ['./report-filter.component.scss'],
})
export class ReportFilterComponent {
  @Output()
  readonly reportFilterChanged = new EventEmitter<ReportFilter>();

  possibleUsers!: User[];
  possibleSubprojects!: SubprojectShallow[];

  readonly filterFormGroup: FormGroup;

  constructor(private userService: UserService, private subprojectService: SubprojectService) {
    this.filterFormGroup = new FormGroup({
      dateRange: new FormGroup({
        begin: new FormControl(moment(), Validators.required),
        end: new FormControl(moment(), Validators.required),
      }),
      projectNumber: new FormControl(null),
      subprojectNumber: new FormControl(null),
      userIds: new FormControl([]),
      commentFilter: new FormControl(''),
    });

    userService.getAll().subscribe((users) => (this.possibleUsers = users));
    subprojectService.getAll().subscribe((subprojects) => (this.possibleSubprojects = subprojects));
  }

  applyFilterClicked(): void {
    this.reportFilterChanged.emit(this.getReportFilter());
  }

  private getReportFilter(): ReportFilter {
    const dateRange = new DateRange();
    dateRange.begin = this.filterFormGroup.get(['dateRange', 'begin'])?.value;
    dateRange.end = this.filterFormGroup.get(['dateRange', 'end'])?.value;

    const reportFilter = new ReportFilter();
    reportFilter.dateRange = dateRange;
    reportFilter.projectNumber = this.filterFormGroup.get('projectNumber')?.value;
    reportFilter.subprojectNumber = this.filterFormGroup.get('subprojectNumber')?.value;
    reportFilter.userIds = this.filterFormGroup.get('userIds')?.value;

    return reportFilter;
  }
}
