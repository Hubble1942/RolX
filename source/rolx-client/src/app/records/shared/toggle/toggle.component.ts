import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'rolx-toggle',
  templateUrl: './toggle.component.html',
  styleUrls: ['./toggle.component.scss'],
})
export class ToggleComponent implements OnInit {
  private startTime?: Date;

  constructor() {}

  ngOnInit(): void {}

  startToggle(): void {
    this.storeStartTime(new Date());
  }

  startToggleDisabled(): boolean {
    return !!this.getStartTime();
  }

  storeStartTime(startTime: Date | undefined): void {
    this.startTime = startTime;
  }

  getStartTime(): Date | undefined {
    return this.startTime;
  }

  stopToggle(): void {
    this.storeStartTime(undefined);
  }

  stopToggleDisabled(): boolean {
    return !this.getStartTime();
  }
}
