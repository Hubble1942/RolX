import { Directive } from '@angular/core';
import { MAT_DATE_FORMATS } from '@angular/material/core';

export const MONTH_FORMAT = {
  parse: {
    dateInput: 'MM.YYYY',
  },
  display: {
    dateInput: 'MM.YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@Directive({
  selector: '[rolxMonthFormat]',
  providers: [{ provide: MAT_DATE_FORMATS, useValue: MONTH_FORMAT }],
})
export class MonthFormatDirective {
}
