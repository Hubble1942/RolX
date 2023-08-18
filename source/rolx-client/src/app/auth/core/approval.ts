import { TransformAsIsoDateTime } from '@app/core/util/iso-date-time';
import { assertDefined } from '@app/core/util/utils';
import { Role } from '@app/users/core/role';
import { User } from '@app/users/core/user';
import { Type } from 'class-transformer';
import * as moment from 'moment';

export class Approval {
  @Type(() => User)
  user!: User;

  bearerToken!: string;

  @TransformAsIsoDateTime()
  expires!: moment.Moment;

  validateModel(): void {
    assertDefined(this, 'user');
    assertDefined(this, 'bearerToken');
    assertDefined(this, 'expires');
  }

  get isExpired(): boolean {
    return this.expires.isBefore(moment().add(5, 'm'));
  }

  get isSupervisor(): boolean {
    return this.user.role >= Role.Supervisor;
  }

  get isBackoffice(): boolean {
    return this.user.role >= Role.Backoffice;
  }

  get isAdministrator(): boolean {
    return this.user.role >= Role.Administrator;
  }
}
