import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {GlobalLoaderComponent} from './components/common/global-loader/global-loader.component';
import {Toast} from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalLoaderComponent, Toast],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'RoboTasker.UI';
}
