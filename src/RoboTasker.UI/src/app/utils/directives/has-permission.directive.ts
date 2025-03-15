import {Directive, inject, Input, input, TemplateRef, ViewContainerRef} from '@angular/core';
import {CurrentUserService} from '../../services/user/current-user.service';

@Directive({
  selector: '[rbHasPermission]'
})
export class HasPermissionDirective {
  private _permission!: string;
  private currentUserService = inject(CurrentUserService);

  @Input('rbHasPermission')
  set hasPermission(permission: string) {
    this._permission = permission;
    this.updateView();
  }

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) {
  }

  private updateView() {
    const hasAccess = this.checkPermission(this._permission);

    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkPermission(permission: string) {
    const user = this.currentUserService.currentUser();
    if (!user) return false;

    return user.permissions.some(p => p.name === permission);
  }
}
