import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {NavigationEnd, RouterOutlet} from '@angular/router';
import {SidebarComponent} from '../sidebar/sidebar.component';
import {TopbarComponent} from '../topbar/topbar.component';
import {LayoutService} from '../../../services/layout/layout.service';
import {BaseComponent} from '../../common/base/base.component';
import {Card} from 'primeng/card';
import {DialogService} from 'primeng/dynamicdialog';
import {DeviceDetectorService} from 'ngx-device-detector';
import {NavbarComponent} from '../navbar/navbar.component';
import {filter, map, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'nb-layout',
  imports: [
    RouterOutlet,
    SidebarComponent,
    TopbarComponent,
    Card,
    NavbarComponent
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss',
  providers: [DialogService],
})
export class LayoutComponent extends BaseComponent implements OnInit {
  private layoutService = inject(LayoutService);
  private deviceDetector = inject(DeviceDetectorService);
  private destroyRef = inject(DestroyRef);

  sidebarOpened = this.layoutService.sidebarOpened;
  wrapToCard = this.layoutService.wrapToCard;

  title = signal<string | null>('Nerbotix');

  ngOnInit() {
    this.router.events.pipe(
      filter(ev => ev instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute;
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      }),
      tap((route) => {
        if (route.snapshot.data['hideTitle'] !== false) {
          this.title.set(route.snapshot.title ?? 'Nerbotix');
        }
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  closeSidebarIfMobile() {
    if (this.deviceDetector.isMobile() && this.sidebarOpened()) {
      this.layoutService.closeSidebar();
    }
  }
}
