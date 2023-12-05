import { Duration } from '@app/core/util/duration';
import { TimeOfDay } from '@app/core/util/time-of-day';
import { RecordEntry } from '@app/records/core/record-entry';

import { ParserResult } from './parser-result';
import { RecordEntryParserUtils } from './record-entry-parser-utils';

class parserState {
  entry: RecordEntry = new RecordEntry();
  success = false;
  errorMsg = '';
}

export class RecordEntryParser {
  static language = {
    oclock: ['uhr'],
    hour: ['stunde', 'stunden'],
    minute: ['minute', 'minuten'],
    comment: ['kommentar', 'kommentare', 'kommentaren'],
    until: ['bis'],
  };

  static getComment(text: string) {
    return RecordEntryParserUtils.getStringAfterFirstWordOfList(text, this.language.comment);
  }

  static getStringBeforeComment(text: string): string {
    return RecordEntryParserUtils.getStringBeforeFirstWordOfList(text, this.language.comment);
  }

  static findFirstTimeString(text: string) {
    const result = RecordEntryParserUtils.findTimeStrings(text);
    return result == null ? undefined : result[0];
  }

  static findSecondTimeString(text: string) {
    const result = RecordEntryParserUtils.findTimeStrings(text);
    return result == null ? undefined : result[1];
  }

  static parseFromTo(text: string, entry: RecordEntry): boolean {
    const fromTimePart = RecordEntryParserUtils.getStringBeforeFirstWordOfList(
      text,
      this.language.until,
    );
    const hour1 = RecordEntryParserUtils.findNthNumber(fromTimePart, 0);
    let minute1 = RecordEntryParserUtils.findNthNumber(fromTimePart, 1);
    minute1 ??= 0;

    const untilTimePart = RecordEntryParserUtils.getStringAfterFirstWordOfList(
      text,
      this.language.until,
    );
    const hour2 = RecordEntryParserUtils.findNthNumber(untilTimePart, 0);
    let minute2 = RecordEntryParserUtils.findNthNumber(untilTimePart, 1);
    minute2 ??= 0;

    if (hour1 === undefined || hour2 === undefined) {
      return false;
    }

    const timeFrom = TimeOfDay.fromHours(hour1 + minute1 / 60.0);
    const timeTo = TimeOfDay.fromHours(hour2 + minute2 / 60.0);

    entry.begin = timeFrom;
    entry.duration = timeTo.sub(timeFrom);

    return true;
  }

  static parseDuration(text: string, entry: RecordEntry): boolean {
    const hasHours = RecordEntryParserUtils.hasOneWordOfList(text, this.language.hour);

    let hours: number | undefined;
    let minutes: number | undefined;
    let foundTime = false;

    if (hasHours) {
      hours = RecordEntryParserUtils.findNthNumber(text, 0);
      const number1 = RecordEntryParserUtils.findNthNumber(text, 1);
      minutes = number1 ?? 0;
      if (hours !== undefined) {
        foundTime = true;
      }
    } else {
      minutes = RecordEntryParserUtils.findNthNumber(text, 0);
      if (minutes !== undefined) {
        foundTime = true;
      }
    }

    if (foundTime) {
      minutes = minutes ?? 0;
      hours = hours ?? 0;
      entry.duration = Duration.fromHours(hours + minutes / 60.0);
      return true;
    }

    return false;
  }

  static parseComment(text: string, state: parserState) {
    if (RecordEntryParserUtils.hasOneDistinctWordOfList(text, this.language.comment)) {
      state.entry.comment = this.getComment(text);
      text = this.getStringBeforeComment(text);
    } else {
      state.entry.comment = '';
    }
    return text;
  }

  static ParseFromToSuccessful(text: string, state: parserState) {
    if (RecordEntryParserUtils.hasOneDistinctWordOfList(text, this.language.until)) {
      if (this.parseFromTo(text, state.entry)) {
        return true;
      }
      state.success = false;
      state.errorMsg = 'Eingabe enthält keine zwei Zeiten wie z.B. "8 Uhr 15 bis 9 Uhr 30"';
    }
    return false;
  }

  static ParseDurationSuccessful(text: string, state: parserState) {
    if (
      RecordEntryParserUtils.hasOneWordOfList(text, this.language.hour.concat(this.language.minute))
    ) {
      if (this.parseDuration(text, state.entry)) {
        return true;
      }
      state.success = false;
      state.errorMsg = 'Eingabe enthält keine Zeitdauer wie z.B. "8 Stunden 15 Minuten"';
    }
    return false;
  }

  static parse(text: string): ParserResult {
    // Spezifikation
    // - Wenn Text das Wort "Stunde" oder "Minute" enthält, gehen wir davon aus, dass im Text eine fixe Zeitdauer vorkommt
    // - Wenn der Text das Wort "bis" enthält, gehen wir davon aus, dass es sich um einen
    //   von-bis Eintrag handelt
    // - Wenn der Text das Wort "Kommentar" enthält, wird alles danach als Kommentar gespeichert

    const state: parserState = new parserState();

    text = this.parseComment(text, state);
    text = RecordEntryParserUtils.replaceWrittenNumbers(text);

    if (this.ParseFromToSuccessful(text, state)) {
      return ParserResult.Success(text, state.entry);
    }

    if (this.ParseDurationSuccessful(text, state)) {
      return ParserResult.Success(text, state.entry);
    }

    if (state.errorMsg === '') {
      state.errorMsg = 'Keine Zeiten gefunden';
    }

    return ParserResult.Fail(text, state.errorMsg);
  }
}
