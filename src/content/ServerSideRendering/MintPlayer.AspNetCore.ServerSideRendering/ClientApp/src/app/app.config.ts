import { ApplicationConfig } from '@angular/core';
import { PreloadAllModules, PreloadingStrategy, provideRouter, withEnabledBlockingInitialNavigation, withPreloading } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes, withPreloading(PreloadAllModules), withEnabledBlockingInitialNavigation()), provideAnimations()]
};
