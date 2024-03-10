import { ApplicationConfig, importProvidersFrom, isDevMode } from '@angular/core';
import { PreloadAllModules, provideRouter, withEnabledBlockingInitialNavigation, withPreloading } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
////#if (UseXsrfProtection)
import { provideHttpClient, withXsrfConfiguration } from '@angular/common/http';
////#else
import { provideHttpClient, withNoXsrfProtection } from '@angular/common/http';
////#endif
////#if (UsePwa)
import { provideServiceWorker } from '@angular/service-worker';
////#endif
import { NgxsModule } from '@ngxs/store';

import { routes } from './app.routes';
import { ApplicationManager } from './states/application/application.manager';

export const appConfig: ApplicationConfig = {
	providers: [
		provideRouter(routes, withPreloading(PreloadAllModules), withEnabledBlockingInitialNavigation()),
////#if (UsePwa)
		provideServiceWorker('ngsw-worker.js', {
			enabled: !isDevMode(),
			registrationStrategy: 'registerWhenStable:30000'
		}),
////#endif
		provideAnimations(),
////#if (UseXsrfProtection)
		provideHttpClient(withXsrfConfiguration({
			cookieName: 'XSRF-TOKEN',
			headerName: 'X-XSRF-TOKEN'
		})),
////#else
		provideHttpClient(withNoXsrfProtection()),
////#endif,
		importProvidersFrom(NgxsModule.forRoot([
			ApplicationManager
		])),
	]
};
