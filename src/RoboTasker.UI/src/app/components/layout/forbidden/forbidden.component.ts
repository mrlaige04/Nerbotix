import { Component } from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {Button} from 'primeng/button';

@Component({
  selector: 'rb-forbidden',
  imports: [
    Button
  ],
  templateUrl: './forbidden.component.html',
  styleUrl: './forbidden.component.scss'
})
export class ForbiddenComponent extends BaseComponent{
  goHome() {
    this.router.navigate(['/']);
  }
}
