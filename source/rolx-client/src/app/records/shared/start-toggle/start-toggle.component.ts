import { Component } from '@angular/core';
import { ToggleService } from '@app/core/persistence/toggle.service';
import * as moment from 'moment';

@Component({
  selector: 'rolx-start-toggle',
  templateUrl: './start-toggle.component.html',
})
export class StartToggleComponent {
  constructor(private toggleService: ToggleService) {}

  get toggleActive(): boolean {
    return this.toggleService.active;
  }

  startToggle() {
    const now = moment();
    this.toggleService.startToggle(now);
  }

  clearToggle() {
    this.toggleService.clearToggle();
  }
}
