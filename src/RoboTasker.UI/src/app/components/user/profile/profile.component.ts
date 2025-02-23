import {Component, inject} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {CurrentUserService} from '../../../services/user/current-user.service';

@Component({
  selector: 'rb-profile',
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent extends BaseComponent {
  private currentUserService = inject(CurrentUserService);

  currentUser = this.currentUserService.currentUser;
}
