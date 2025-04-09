import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'nb-users-wrapper',
  imports: [
    RouterOutlet
  ],
  templateUrl: './users-wrapper.component.html',
  styleUrl: './users-wrapper.component.scss'
})
export class UsersWrapperComponent {

}
