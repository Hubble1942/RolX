import { assertDefined } from '@app/core/util/utils';

export class ReportRangeType {
  id!: string;
  label!: string;
  hasCustomStart!: boolean;
  hasCustomEnd!: boolean;
  hasCustomEndMonth!: boolean;
  validateModel() {
    assertDefined(this, 'id');
    assertDefined(this, 'label');
    assertDefined(this, 'hasCustomStart');
    assertDefined(this, 'hasCustomEnd');
    assertDefined(this, 'hasCustomEndMonth');
  }
}
