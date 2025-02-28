import {Component, inject} from '@angular/core';
import {GlobalLoaderService} from '../../../services/common/global-loader.service';
import {ActivatedRoute, Router} from '@angular/router';
import {NotificationService} from '../../../services/common/notification.service';
import {DialogService} from 'primeng/dynamicdialog';
import {ConfirmationService} from 'primeng/api';

@Component({
  selector: 'rb-base',
  imports: [],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss',
  providers: [DialogService]
})
export class BaseComponent {
  private globalLoader = inject(GlobalLoaderService);
  protected router = inject(Router);
  protected activatedRoute = inject(ActivatedRoute);
  protected notificationService = inject(NotificationService);
  protected dialogService = inject(DialogService);
  protected confirmationService = inject(ConfirmationService);

  showLoader() {
    this.globalLoader.showLoader();
  }

  hideLoader() {
    this.globalLoader.hideLoader();
  }
}
