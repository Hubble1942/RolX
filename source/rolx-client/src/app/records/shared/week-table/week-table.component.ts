import { Component, Input, OnInit, OnChanges, Output, EventEmitter } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ErrorService } from '@app/core/error/error.service';
import { ListService } from '@app/core/persistence/list-service';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { Record } from '@app/records/core/record';
import { WorkRecordService } from '@app/records/core/work-record.service';
import { User } from '@app/users/core/user';
import * as moment from 'moment';
import { TableVirtualScrollDataSource } from 'ng-table-virtual-scroll';
import { filter } from 'rxjs';

import {
  InvalidEntriesDialogComponent,
  InvalidEntriesDialogData,
} from '../invalid-entries-dialog/invalid-entries-dialog.component';

import { TreeNode } from './tree-node';

@Component({
  selector: 'rolx-week-table',
  templateUrl: './week-table.component.html',
  styleUrls: ['./week-table.component.scss'],
})
export class WeekTableComponent implements OnInit, OnChanges {
  private readonly expandedNodes = new Set<string>(
    this.listService.get<string>('expanded-nodes', []),
  );

  private readonly allWeekdays = [
    'monday',
    'tuesday',
    'wednesday',
    'thursday',
    'friday',
    'saturday',
    'sunday',
  ];

  private _showWeekends = false;
  private _activities: Activity[] = [];
  private _records: Record[] = [];

  weekdays: string[] = [];
  displayedColumns: string[] = [];

  @Input()
  get records() {
    return this._records;
  }
  set records(value) {
    this._records = value;
    this.todaysRecord = this.records.find((r) => r.isToday);
  }

  @Input()
  user!: User;

  @Input()
  showToggle = false;

  @Input()
  isCurrentUser = false;

  @Input()
  asTreeView = false;

  @Input()
  forceExpandTree = false;

  @Input()
  get showWeekends() {
    return this._showWeekends;
  }
  set showWeekends(value: boolean) {
    this._showWeekends = value;

    this.weekdays = this.showWeekends
      ? this.allWeekdays
      : this.allWeekdays.slice(0, this.allWeekdays.length - 2);
    this.displayedColumns = ['activity', ...this.weekdays];
  }

  @Input()
  get activities() {
    return this._activities;
  }
  set activities(value: Activity[]) {
    this._activities = value.sort((a, b) => a.fullName.localeCompare(b.fullName));
  }

  @Output()
  readonly recordChanged = new EventEmitter<Record>();

  readonly dataSource = new TableVirtualScrollDataSource<TreeNode | Activity>();

  todaysRecord?: Record;
  recordIsInvalid: boolean[] = [];

  constructor(
    private workRecordService: WorkRecordService,
    private errorService: ErrorService,
    private readonly dialog: MatDialog,
    private listService: ListService,
  ) {
    this.showWeekends = false;
  }

  isActivity(item: TreeNode | Activity | null): boolean {
    return item instanceof Activity;
  }

  isTreeNode(item: TreeNode | Activity | null): boolean {
    return item instanceof TreeNode;
  }

  ngOnInit() {
    assertDefined(this, 'user');
  }

  ngOnChanges() {
    this.recordIsInvalid = this.records.map((r) =>
      this.activities.some((a) => this.isRecordInvalidForActivity(r, a)),
    );
    this.update();
  }

  trackByNode(index: number, node: TreeNode | Activity | null): number {
    if (node instanceof TreeNode) {
      return +((node.parentNumber ?? '') + node.number).replace('#', '');
    } else if (node instanceof Activity) {
      return node.id;
    }
    return -1;
  }

  toggleExpand(node: TreeNode) {
    node.isExpanded = !node.isExpanded;
    if (node.isExpanded) {
      this.expandedNodes.add((node.parentNumber ?? '') + node.number);
    } else {
      this.expandedNodes.delete((node.parentNumber ?? '') + node.number);
    }
    this.update();
  }

  isToggleShownFor(node: TreeNode | Activity | null) {
    return this.showToggle && this.isActivity(node) && (node as Activity).isOpenAt(moment());
  }

  submit(record: Record, index: number) {
    this.workRecordService.update(this.user.id, record).subscribe({
      next: (r) => {
        this.records[index] = r;
        // API only succeeds if the record is valid
        this.recordIsInvalid[index] = false;
        this.recordChanged.next(r);
      },
      error: (err) => {
        console.error(err);
        this.errorService.notifyGeneralError();
      },
    });
  }

  submitToggleRecord(record: Record): void {
    this.submit(
      record,
      this.records.findIndex((r) => r.isToday),
    );
  }

  hasInvalidRecord(activity: Activity | null): boolean {
    return (
      activity != null && this.records.some((r) => this.isRecordInvalidForActivity(r, activity))
    );
  }

  isRecordInvalidForActivity(record: Record, activity: Activity): boolean {
    return (
      !activity.isOpenAt(record.date) && record.entries.some((e) => e.activityId === activity.id)
    );
  }

  openInvalidEntryDialog(record: Record, index: number) {
    const data: InvalidEntriesDialogData = {
      record,
      offendingActivities: this.activities.filter((a) =>
        this.isRecordInvalidForActivity(record, a),
      ),
    };
    this.dialog
      .open(InvalidEntriesDialogComponent, {
        closeOnNavigation: true,
        data,
      })
      .afterClosed()
      .pipe(filter((r) => r != null))
      .subscribe((r) => this.submit(r, index));
  }

  private createTree(value: Activity[]): TreeNode[] {
    const baseNodes: TreeNode[] = [];
    for (const activity of value) {
      const parsedNumbers = activity.fullNumber.split('.');
      let baseNode = baseNodes.find((n) => n.number === parsedNumbers[0]);
      if (baseNode == null) {
        baseNode = new TreeNode(
          `${activity.customerName} - ${activity.projectName}`,
          parsedNumbers[0],
        );
        baseNode.isExpanded = this.expandedNodes.has(baseNode.number) || this.forceExpandTree;
        baseNodes.push(baseNode);
      }

      let subNode = baseNode.children
        .filter((c) => c instanceof TreeNode)
        .find((c) => c.number === parsedNumbers[1]) as TreeNode | undefined;
      if (subNode == null) {
        subNode = new TreeNode(activity.subprojectName, parsedNumbers[1]);
        subNode.parentNumber = baseNode.number;
        subNode.isExpanded =
          this.expandedNodes.has(baseNode.number + subNode.number) || this.forceExpandTree;
        baseNode.children.push(subNode);
      }

      subNode.children.push(activity);
    }
    return baseNodes;
  }

  private *flattenTree(nodes: (TreeNode | Activity)[]): IterableIterator<TreeNode | Activity> {
    for (const node of nodes) {
      yield node;
      if (node instanceof TreeNode && node.isExpanded) {
        for (const child of this.flattenTree(node.children)) {
          yield child;
        }
      }
    }
  }

  private update() {
    if (this.asTreeView) {
      this.dataSource.data = Array.from(this.flattenTree(this.createTree(this.activities)));
      this.listService.set<string>('expanded-nodes', Array.from(this.expandedNodes.keys()));
    } else {
      this.dataSource.data = this.activities;
    }
  }
}
