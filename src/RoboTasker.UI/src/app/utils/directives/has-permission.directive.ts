import {Directive, inject, Input, input, TemplateRef, ViewContainerRef} from '@angular/core';
import {CurrentUserService} from '../../services/user/current-user.service';

@Directive({
  selector: '[rbHasPermission]'
})
export class HasPermissionDirective {
  private _permission: string | undefined;
  private currentUserService = inject(CurrentUserService);

  @Input('rbHasPermission')
  set hasPermission(permission: string | undefined) {
    this._permission = permission;
    this.updateView();
  }

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) {
  }

  private updateView() {
    const hasAccess = !this._permission || this.checkPermission(this._permission);

    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkPermission(permission: string | undefined) {
    const user = this.currentUserService.currentUser();
    if (!user) return false;

    return user.permissions.some(p => p.name === permission);
  }
}
