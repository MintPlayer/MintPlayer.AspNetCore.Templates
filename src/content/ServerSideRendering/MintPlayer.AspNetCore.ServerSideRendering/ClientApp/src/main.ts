import { bootstrapApplication } from '@angular/platform-browser';
import { appBrowserConfig } from './app/app.config.browser';
import { AppComponent } from './app/app.component';


bootstrapApplication(AppComponent, appBrowserConfig)
  .catch((err) => console.error(err));
