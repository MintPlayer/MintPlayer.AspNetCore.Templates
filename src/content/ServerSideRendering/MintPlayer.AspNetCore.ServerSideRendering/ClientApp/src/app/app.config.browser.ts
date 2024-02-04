import { mergeApplicationConfig, ApplicationConfig } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';
import { appConfig } from './app.config';
import { DATA_FROM_SERVER } from './providers/data-from-server';

const getBaseUrl = () => {
  return document.getElementsByTagName('base')[0].href.slice(0, -1);
}

const browserConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    { provide: 'MESSAGE', useValue: 'B' },
    { provide: APP_BASE_HREF, useFactory: getBaseUrl },
  ]
};

export const appBrowserConfig = mergeApplicationConfig(appConfig, browserConfig);
