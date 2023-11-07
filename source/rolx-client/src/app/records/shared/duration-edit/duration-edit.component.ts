import {
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ShowOnDirtyErrorStateMatcher } from '@angular/material/core';
import { FlagService } from '@app/core/persistence/flag-service';
import { Duration } from '@app/core/util/duration';
import { DurationValidators } from '@app/core/util/duration.validators';
import { TimeFormControl } from '@app/core/util/time-form-control';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';

@Component({
  selector: 'rolx-duration-edit',
  templateUrl: './duration-edit.component.html',
  styleUrls: ['./duration-edit.component.scss'],
})
export class DurationEditComponent implements OnInit, OnDestroy {
  private readonly changedSubject = new Subject<Duration>();
  private valueShadow = Duration.Zero;
  private subscriptions: Subscription = new Subscription();

  @ViewChild('input')
  private inputElement?: ElementRef;

  @Input()
  get value() {
    return this.valueShadow;
  }
  set value(value: Duration) {
    this.valueShadow = value ? value : Duration.Zero;
    this.cancel();
  }

  @Output()
  more = new EventEmitter();

  @Output()
  startRecording = new EventEmitter();

  @Output()
  changed = this.changedSubject.pipe(
    debounceTime(20), // filter multiple events cause by enter-key leading to blur
  );

  readonly errorStateMatcher = new ShowOnDirtyErrorStateMatcher();

  readonly control = TimeFormControl.createDuration(null, DurationValidators.min(Duration.Zero));
  readonly form = new FormGroup({
    duration: this.control,
  });

  get hasVoice() {
    return this.value.isZero && this.flagService.get('voiceInput', false);
  }

  constructor(private readonly flagService: FlagService) {
    this.subscriptions.add(
      this.flagService.flagChanged$
        .pipe(filter((change) => change.flag === 'formatDurationsAsDecimal'))
        .subscribe(() => this.cancel()),
    );
  }

  ngOnInit() {
    this.cancel();
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  enter() {
    this.inputElement?.nativeElement.focus();
  }

  checkIfLeavingIsAllowed() {
    return this.control.valid;
  }

  commit() {
    if (!this.control.dirty) {
      return;
    }

    if (this.form.invalid) {
      this.cancel();
      return;
    }

    const editedValue = this.control.value ? this.control.value : Duration.Zero;
    if (this.value.isSame(editedValue)) {
      this.cancel();
      return;
    }

    this.changedSubject.next(editedValue);
  }

  cancel() {
    this.control.reset(!this.value.isZero ? this.value : '');
  }
}
