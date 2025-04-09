import {Component, HostBinding, inject} from '@angular/core';
import {GlobalLoaderService} from '../../../services/common/global-loader.service';

@Component({
  selector: 'nb-global-loader',
  imports: [],
  templateUrl: './global-loader.component.html',
  styleUrl: './global-loader.component.scss'
})
export class GlobalLoaderComponent {
  private globalLoaderService = inject(GlobalLoaderService);
  showLoader = this.globalLoaderService.isShowLoader;

  @HostBinding('style.display') get display() {
    return this.showLoader() ? 'block' : 'none';
  }
}
