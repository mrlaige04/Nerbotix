import { Component } from '@angular/core';
import {RouterOutlet} from '@angular/router';

@Component({
  selector: 'rb-tasks-wrapper',
  imports: [
    RouterOutlet
  ],
  templateUrl: './tasks-wrapper.component.html',
  styleUrl: './tasks-wrapper.component.scss'
})
export class TasksWrapperComponent {

}
