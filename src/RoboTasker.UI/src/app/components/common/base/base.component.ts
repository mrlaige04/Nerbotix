import {Component, inject} from '@angular/core';
import {GlobalLoaderService} from '../../../services/common/global-loader.service';
import {Router} from '@angular/router';

@Component({
  selector: 'rb-base',
  imports: [],
  templateUrl: './base.component.html',
  styleUrl: './base.component.scss'
})
export class BaseComponent {
  private globalLoader = inject(GlobalLoaderService);
  protected router = inject(Router);

  showLoader() {
    this.globalLoader.showLoader();
  }

  hideLoader() {
    this.globalLoader.hideLoader();
  }
}
