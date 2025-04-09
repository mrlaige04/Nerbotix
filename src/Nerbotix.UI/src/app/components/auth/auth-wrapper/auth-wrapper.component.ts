import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {Card} from 'primeng/card';
import {BaseComponent} from '../../common/base/base.component';
import {DialogService} from 'primeng/dynamicdialog';

@Component({
  selector: 'nb-auth-wrapper',
  imports: [
    RouterOutlet,
    Card,
  ],
  templateUrl: './auth-wrapper.component.html',
  styleUrl: './auth-wrapper.component.scss',
  providers: [DialogService]
})
export class AuthWrapperComponent extends BaseComponent {

}
