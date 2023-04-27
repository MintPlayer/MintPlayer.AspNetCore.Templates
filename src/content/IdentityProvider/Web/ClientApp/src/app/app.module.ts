import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { BsNavbarModule } from '@mintplayer/ng-bootstrap/navbar';
import { AdvancedRouterModule, QueryParamsConfig, QUERY_PARAMS_CONFIG } from '@mintplayer/ng-router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApplicationManager } from './states/application/application.manager';

@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
		BrowserModule,
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

		AppRoutingModule
	],
	providers: [{
		provide: QUERY_PARAMS_CONFIG,
		useValue: <QueryParamsConfig>{
			lang: 'preserve',
			return: ''
		}
	}],
	bootstrap: [AppComponent]
})
export class AppModule { }
