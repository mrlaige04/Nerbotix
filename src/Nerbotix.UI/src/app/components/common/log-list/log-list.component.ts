import {Component, computed, input} from '@angular/core';
import {BaseComponent} from '../base/base.component';
import { Log } from '../../../models/logging/log';
import {DatePipe} from '@angular/common';
import {LogLevel} from '../../../enums/log-level';

@Component({
  selector: 'nb-log-list',
  imports: [
    DatePipe
  ],
  templateUrl: './log-list.component.html',
  styleUrl: './log-list.component.scss'
})
export class LogListComponent extends BaseComponent {
  logs = input.required<Log[]>();

  protected readonly LogLevel = LogLevel;
}
