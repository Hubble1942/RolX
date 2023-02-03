import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import * as moment from 'moment';

export class ReportEntry {
  @TransformAsIsoDate()
  date!: moment.Moment;

  projectNumber!: number;
  customerName!: string;
  projectName!: boolean;
  subprojectNumber!: number;
  subprojectName!: string;
  activityNumber!: number;
  activityName!: string;
  billabilityName!: string;
  userName!: string;

  @TransformAsDuration()
  duration!: Duration;

  comment!: string;

  validateModel(): void {
    assertDefined(this, 'date');
    assertDefined(this, 'projectNumber');
    assertDefined(this, 'customerName');
    assertDefined(this, 'projectName');
    assertDefined(this, 'subprojectNumber');
    assertDefined(this, 'subprojectName');
    assertDefined(this, 'activityNumber');
    assertDefined(this, 'activityName');
    assertDefined(this, 'billabilityName');
    assertDefined(this, 'userName');
    assertDefined(this, 'duration');
    assertDefined(this, 'comment');
  }
}
