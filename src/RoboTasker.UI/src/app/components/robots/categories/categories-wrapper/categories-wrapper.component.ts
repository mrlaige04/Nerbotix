import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {BaseComponent} from '../../../common/base/base.component';

@Component({
  selector: 'rb-categories-wrapper',
  imports: [
    RouterOutlet
  ],
  templateUrl: './categories-wrapper.component.html',
  styleUrl: './categories-wrapper.component.scss'
})
export class CategoriesWrapperComponent extends BaseComponent {

}
