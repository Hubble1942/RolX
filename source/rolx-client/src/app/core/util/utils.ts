import * as moment from 'moment';

export const assertDefined = <T>(item: T, ...propertyNames: (keyof T)[]): void => {
  for (const propertyName of propertyNames) {
    if (item[propertyName] === undefined) {
      throw new Error(`${String(propertyName)} must be defined`);
    }
  }
};

export const momentEquals = (a?: moment.Moment, b?: moment.Moment) => {
  if (a === undefined && b === undefined) {
    return true;
  }
  if (a === undefined || b === undefined) {
    return false;
  }
  return a.isSame(b);
};
