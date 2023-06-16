import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import { SubprojectShallow } from '@app/projects/core/subproject-shallow';
import { Type } from 'class-transformer';
import * as moment from 'moment';

import { Activity } from './activity';

export class Subproject extends SubprojectShallow {
  fullName!: string;
  projectNumber!: number;
  number!: number;
  managerId?: string;
  deputyManagerId?: string;
  architectId?: string;
  isOverBudget!: boolean;
  isOverPlanned!: boolean;

  @TransformAsIsoDate()
  startDate!: moment.Moment | null;

  @TransformAsDuration()
  budget!: Duration;

  @TransformAsDuration()
  planned!: Duration;

  @TransformAsDuration()
  actual!: Duration;

  budgetConsumedFraction!: number;
  plannedConsumedFraction!: number;

  @Type(() => Activity)
  activities: Activity[] = [];

  override validateModel(): void {
    super.validateModel();

    assertDefined(this, 'fullName');
    assertDefined(this, 'projectNumber');
    assertDefined(this, 'number');
    assertDefined(this, 'isOverBudget');
    assertDefined(this, 'budget');
    assertDefined(this, 'planned');
    assertDefined(this, 'actual');
    assertDefined(this, 'budgetConsumedFraction');
    assertDefined(this, 'plannedConsumedFraction');
    assertDefined(this, 'activities');

    this.activities.forEach((a) => a.validateModel());
  }

  get nextNumber() {
    return Math.max(...this.activities.map((ph) => ph.number), 0) + 1;
  }

  addActivity(): Activity {
    const activity = new Activity();
    activity.startDate = moment();
    activity.number = this.nextNumber;
    activity.fullName = '';
    activity.fullNumber = '';
    activity.allSubprojectNames = '';
    activity.billabilityName = '';
    activity.customerName = '';
    activity.projectName = '';
    activity.subprojectName = '';
    this.activities.push(activity);
    return activity;
  }
}
