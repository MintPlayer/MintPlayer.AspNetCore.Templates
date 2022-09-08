import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { BsNavbarModule } from '@mintplayer/ng-bootstrap';
import { AdvancedRouterModule } from '@mintplayer/ng-router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApplicationManager } from './states/application/application.manager';
////#if (UsePwa)
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
////#endif

@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
////#if (UseServerSideRendering)
		BrowserModule.withServerTransition({ appId: 'serverApp' }),
////#else
		BrowserModule,
////#endif
		BrowserAnimationsModule,
		HttpClientModule,
////#if (UseXsrfProtection)
		HttpClientXsrfModule.withOptions({
			cookieName: 'XSRF-TOKEN',
			headerName: 'X-XSRF-TOKEN'
		}),
////#endif
		NgxsModule.forRoot([
			ApplicationManager
		]),
		AdvancedRouterModule,
		BsNavbarModule,
////#if (UsePwa)
		ServiceWorkerModule.register('ngsw-worker.js', {
			enabled: environment.production,
			//// Register the ServiceWorker as soon as the application is stable
			//// or after 30 seconds (whichever comes first).
			registrationStrategy: 'registerWhenStable:30000'
		}),
////#endif
		AppRoutingModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
