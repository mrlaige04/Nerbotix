import {InjectionToken, Provider} from '@angular/core';
import {environment} from '../../environments/environment';

export interface ApiConfig {
  url: string;
}

export const apiConfig = new InjectionToken("API_CONFIG");
const getConfig = () => environment.api;

export const apiConfigProvider: Provider = {
  provide: apiConfig,
  useFactory: getConfig
};
