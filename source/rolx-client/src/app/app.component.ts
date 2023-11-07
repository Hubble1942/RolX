import { Overlay, OverlayConfig, OverlayContainer, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import {
  Component,
  HostBinding,
  OnDestroy,
  OnInit,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { PendingRequestService } from '@app/core/pending-request/pending-request.service';
import { FlagService } from '@app/core/persistence/flag-service';
import { Theme } from '@app/core/theme/theme';
import { ThemeService } from '@app/core/theme/theme.service';
import { Duration } from '@app/core/util/duration';
import { Subscription } from 'rxjs';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'rolx-root',
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit, OnDestroy {
  private static readonly ThemeClasses = ['dark-theme', 'bright-theme'];
  private readonly subscriptions = new Subscription();
  private overlayRef: OverlayRef | null = null;

  @ViewChild('requestPendingSpinner') requestPendingSpinner?: TemplateRef<any>;

  @HostBinding('class') componentCssClass: any;

  constructor(
    private readonly pendingRequestService: PendingRequestService,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly overlay: Overlay,
    public readonly themeService: ThemeService,
    private readonly flagService: FlagService,
    public readonly overlayContainer: OverlayContainer,
  ) {}

  ngOnInit() {
    this.subscriptions.add(this.themeService.currentTheme$.subscribe((t) => this.applyTheme(t)));
    this.subscriptions.add(
      this.pendingRequestService.hasOverdueRequest$.subscribe((v) =>
        v ? this.showOverlay() : this.hideOverlay(),
      ),
    );

    Duration.formatAsDecimal = this.flagService.get('formatDurationsAsDecimal', false);
    this.subscriptions.add(
      this.flagService.flagChanged$
        .pipe(filter((change) => change.flag === 'formatDurationsAsDecimal'))
        .subscribe((change) => (Duration.formatAsDecimal = change.state)),
    );
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }

  private applyTheme(theme: Theme) {
    const themeClass = AppComponent.ThemeClasses[theme];
    this.componentCssClass = themeClass;

    const overlayClassList = this.overlayContainer.getContainerElement().classList;
    overlayClassList.remove(...AppComponent.ThemeClasses);
    overlayClassList.add(themeClass);
  }

  private showOverlay() {
    if (this.overlayRef != null) {
      return;
    }

    if (!this.requestPendingSpinner) {
      console.warn('requestPendingSpinner is undefined');
      return;
    }

    const overlayConfig = new OverlayConfig({
      hasBackdrop: true,
      scrollStrategy: this.overlay.scrollStrategies.block(),
      positionStrategy: this.overlay.position().global().centerHorizontally().centerVertically(),
    });
    const overlayRef = this.overlay.create(overlayConfig);
    const loadingPortal = new TemplatePortal(this.requestPendingSpinner, this.viewContainerRef);
    overlayRef.attach(loadingPortal);
    this.overlayRef = overlayRef;
  }

  private hideOverlay() {
    this.overlayRef?.dispose();
    this.overlayRef = null;
  }
}
