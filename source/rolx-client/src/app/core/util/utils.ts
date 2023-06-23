import * as moment from 'moment';

export const assertDefined = (item: any, propertyName: string): void => {
  if (item[propertyName] === undefined) {
    console.log(item, propertyName);
    throw new Error(propertyName + ' must be defined');
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
