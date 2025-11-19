import { APP_INITIALIZER } from '@angular/core';
import { ConfigService } from './services/config.service';

export function initializeApp(configService: ConfigService) {
  return () => configService.loadConfig();
}

export const appInitializerProvider = {
  provide: APP_INITIALIZER,
  useFactory: initializeApp,
  deps: [ConfigService],
  multi: true
};
