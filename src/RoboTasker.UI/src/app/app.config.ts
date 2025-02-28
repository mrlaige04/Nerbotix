import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {providePrimeNG} from 'primeng/config';
import {MyPreset} from '../mytheme';
import {apiConfigProvider} from './config/http.config';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {passTokenInterceptor} from './utils/interceptors/pass-token.interceptor';
import {ConfirmationService, MessageService} from 'primeng/api';
import {handleUnauthorizedInterceptor} from './utils/interceptors/handle-unauthorized.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: '.dark-theme'
        }
      },
      ripple: true
    }),
    apiConfigProvider,
    provideHttpClient(withInterceptors([ passTokenInterceptor, handleUnauthorizedInterceptor ])),
    MessageService,
    ConfirmationService
  ]
};
