import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StopToggleDialogComponent } from './stop-toggle-dialog.component';

describe('StopToggleDialogComponent', () => {
  let component: StopToggleDialogComponent;
  let fixture: ComponentFixture<StopToggleDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StopToggleDialogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StopToggleDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
