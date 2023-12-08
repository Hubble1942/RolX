import { Duration, TransformAsDuration } from '@app/core/util/duration';
import { TransformAsIsoDate } from '@app/core/util/iso-date';
import { assertDefined } from '@app/core/util/utils';
import { WorkItemGroup } from '@app/reports/core/work-item-group';
import { BalanceCorrection } from '@app/users/core/balance-correction';
import { PartTimeSetting } from '@app/users/core/part-time-setting';
import { User } from '@app/users/core/user';
import { Type } from 'class-transformer';
import * as moment from 'moment';

export class UserMonthReport {
  @Type(() => User)
  user!: User;

  @TransformAsIsoDate()
  month!: moment.Moment;

  @Type(() => PartTimeSetting)
  partTimeSettings!: PartTimeSetting[];

  @Type(() => BalanceCorrection)
  balanceCorrections!: BalanceCorrection[];

  @TransformAsDuration()
  overtime!: Duration;

  @TransformAsDuration()
  overtimeDelta!: Duration;

  @TransformAsDuration()
  vacation!: Duration;

  @TransformAsDuration()
  vacationDelta!: Duration;

  vacationDays!: number | null;
  vacationDeltaDays!: number | null;

  @Type(() => WorkItemGroup)
  workItemGroups!: WorkItemGroup[];

  validateModel(): void {
    assertDefined(this, 'user');
    assertDefined(this, 'month');
    assertDefined(this, 'partTimeSettings');
    assertDefined(this, 'balanceCorrections');
    assertDefined(this, 'overtime');
    assertDefined(this, 'overtimeDelta');
    assertDefined(this, 'vacation');
    assertDefined(this, 'vacationDays');
    assertDefined(this, 'vacationDelta');
    assertDefined(this, 'vacationDeltaDays');
    assertDefined(this, 'workItemGroups');

    this.partTimeSettings.forEach((o) => o.validateModel());
    this.balanceCorrections.forEach((o) => o.validateModel());
    this.workItemGroups.forEach((o) => o.validateModel());
  }
}
