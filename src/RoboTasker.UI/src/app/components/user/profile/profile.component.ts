import {Component} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';

@Component({
  selector: 'rb-profile',
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent extends BaseComponent {
  currentUser = this.currentUserService.currentUser;
}
