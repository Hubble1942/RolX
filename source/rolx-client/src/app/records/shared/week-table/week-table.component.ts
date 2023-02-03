import { Component, ElementRef, Input, OnDestroy, OnInit, OnChanges } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ErrorService } from '@app/core/error/error.service';
import { ListService } from '@app/core/persistence/list-service';
import { assertDefined } from '@app/core/util/utils';
import { Activity } from '@app/projects/core/activity';
import { FavouriteActivityService } from '@app/projects/core/favourite-activity.service';
import { Record } from '@app/records/core/record';
import { WorkRecordService } from '@app/records/core/work-record.service';
import { User } from '@app/users/core/user';
import * as moment from 'moment';
import { filter, Subscription } from 'rxjs';

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
export class WeekTableComponent implements OnInit, OnDestroy, OnChanges {
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

  private _isCurrentUser = false;
  private _showWeekends = false;
  private _asTreeView = false;
  private inputActivities: Activity[] = [];
  private favouriteActivities: Activity[] = [];
  private homegrownActivities: Activity[] = [];
  private favouritesSubscription?: Subscription;

  weekdays: string[] = [];
  displayedColumns: string[] = [];

  @Input()
  records: Record[] = [];

  @Input()
  user!: User;

  @Input()
  showToggle = false;

  @Input()
  get isCurrentUser(): boolean {
    return this._isCurrentUser;
  }
  set isCurrentUser(value: boolean) {
    if (value === this._isCurrentUser) {
      return;
    }

    this._isCurrentUser = value;

    if (this._isCurrentUser) {
      this.favouritesSubscription = this.favouriteActivityService.favourites$.subscribe(
        (phs) => (this.favourites = phs),
      );
    } else {
      this.favouritesSubscription?.unsubscribe();
      this.favourites = [];
    }
  }

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
  get asTreeView() {
    return this._asTreeView;
  }
  set asTreeView(value: boolean) {
    this._asTreeView = value;
    this.update();
  }

  allActivities: Activity[] = [];
  items: (TreeNode | Activity | null)[] = [];
  tree: TreeNode[] = [];
  recordIsInvalid: boolean[] = [];

  isAddingActivity = false;

  constructor(
    private favouriteActivityService: FavouriteActivityService,
    private workRecordService: WorkRecordService,
    private errorService: ErrorService,
    private elementRef: ElementRef,
    private readonly dialog: MatDialog,
    private listService: ListService,
  ) {
    this.showWeekends = false;
  }

  @Input()
  get activities(): Activity[] {
    return this.inputActivities;
  }
  set activities(value: Activity[]) {
    this.inputActivities = value.filter((activity) =>
      this.records.some(
        (record) =>
          activity.isOpenAt(record.date) || this.isRecordInvalidForActivity(record, activity),
      ),
    );
    this.homegrownActivities = [];
    this.isAddingActivity = false;

    this.update();
  }

  get todaysRecord(): Record | undefined {
    return this.records.find((r) => r.isToday);
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
  }

  ngOnDestroy() {
    this.favouritesSubscription?.unsubscribe();
  }

  startAdding() {
    this.isAddingActivity = true;
    this.update();
    setTimeout(() => this.scrollToBottom());
  }

  addHomegrown(activity: Activity) {
    this.isAddingActivity = false;
    this.homegrownActivities.push(activity);
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

  private set favourites(value: Activity[]) {
    this.favouriteActivities = value;
    this.update();
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
        baseNode.isExpanded = this.expandedNodes.has(baseNode.number);
        baseNodes.push(baseNode);
      }

      let subNode = baseNode.children
        .filter((c) => c instanceof TreeNode)
        .find((c) => c.number === parsedNumbers[1]) as TreeNode | undefined;
      if (subNode == null) {
        subNode = new TreeNode(activity.subprojectName, parsedNumbers[1]);
        subNode.parentNumber = baseNode.number;
        subNode.isExpanded = this.expandedNodes.has(baseNode.number + subNode.number);
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
    const localActivitiesIds = new Set<number>(
      [...this.inputActivities, ...this.homegrownActivities].map((ph) => ph.id),
    );

    const nonLocalFavourites = this.favouriteActivities.filter(
      (ph) => !localActivitiesIds.has(ph.id),
    );

    const sortedActivities = [...this.inputActivities, ...nonLocalFavourites].sort((a, b) =>
      a.fullName.localeCompare(b.fullName),
    );

    this.allActivities = [...sortedActivities, ...this.homegrownActivities];

    if (this.asTreeView) {
      this.tree = this.createTree(this.allActivities);
      const tempItems = Array.from(this.flattenTree(this.tree));
      this.items = this.isAddingActivity ? [...tempItems, null] : tempItems;
      this.listService.set<string>('expanded-nodes', Array.from(this.expandedNodes.keys()));
    } else {
      this.items = this.isAddingActivity ? [...this.allActivities, null] : this.allActivities;
    }
  }

  private scrollToBottom() {
    try {
      this.elementRef.nativeElement.scroll({
        top: this.elementRef.nativeElement.scrollHeight,
        behavior: 'instant',
      });
    } catch (err) {}
  }
}
