import {Directive, inject, Input, TemplateRef, ViewContainerRef} from '@angular/core';
import {CurrentUserService} from '../../services/user/current-user.service';

@Directive({
  selector: '[rbHasRole]'
})
export class HasRoleDirective {
  private _role: string | undefined;
  private currentUserService = inject(CurrentUserService);

  @Input('rbHasRole')
  set hasRole(role: string | undefined) {
    this._role = role;
    this.updateView();
  }

  constructor(private templateRef: TemplateRef<any>, private viewContainer: ViewContainerRef) { }

  private updateView() {
    const hasAccess = !this._role || this.checkRole(this._role);

    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }

  private checkRole(role: string | undefined) {
    const user = this.currentUserService.currentUser();
    if (!user) return false;

    return user.roles.some(r => r.name === role);
  }
}
