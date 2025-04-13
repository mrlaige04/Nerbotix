import {Component, computed, inject, input, InputSignal} from '@angular/core';
import {NgxEchartsDirective} from 'ngx-echarts';
import {EChartsCoreOption} from 'echarts/core';
import {UiSettingsService} from '../../../services/layout/ui-settings.service';

@Component({
  selector: 'nb-base-analytic',
  imports: [
    NgxEchartsDirective
  ],
  templateUrl: './base-analytic.component.html',
  styleUrl: './base-analytic.component.scss'
})
export class BaseAnalyticComponent {
  private uiSettingsService = inject(UiSettingsService);
  options: InputSignal<EChartsCoreOption> = input.required<EChartsCoreOption>();
  isLoading = input<boolean>(false);

  finalOptions = computed(() => ({
    ...this.options(),
    grid: {
      left: 16,
      top: 26,
      right: 16,
      bottom: 12,
      containLabel: true
    }
  }));

  isDarkTheme = this.uiSettingsService.theme() === 'dark';
}
