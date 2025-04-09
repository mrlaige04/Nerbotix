import {Directive, inject, TemplateRef, ViewContainerRef} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';

@Directive({
  selector: '[nbIsSuperAdmin]'
})
export class IsSuperAdminDirective {
  private authService = inject(AuthService);

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) {
    this.updateView();
  }

  private updateView() {
    const isSuperAdmin = this.authService.isSuperAdmin();
    if (isSuperAdmin) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
