import { Component, Input } from '@angular/core';

@Component({
  selector: 'rolx-progress-bar',
  templateUrl: './progress-bar.component.html',
  styleUrls: ['./progress-bar.component.scss'],
})
export class ProgressBarComponent {
  @Input() value = 0;
  @Input() color = 'primary';
}
