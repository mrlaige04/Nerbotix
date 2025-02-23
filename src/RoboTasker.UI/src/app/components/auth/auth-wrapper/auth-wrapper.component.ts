import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {Card} from 'primeng/card';
import {BaseComponent} from '../../common/base/base.component';

@Component({
  selector: 'rb-auth-wrapper',
  imports: [
    RouterOutlet,
    Card,
  ],
  templateUrl: './auth-wrapper.component.html',
  styleUrl: './auth-wrapper.component.scss'
})
export class AuthWrapperComponent extends BaseComponent {

}
