import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HttpClientXsrfModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { BsNavbarModule } from '@mintplayer/ng-bootstrap';
import { AdvancedRouterModule, QueryParamsConfig, QUERY_PARAMS_CONFIG } from '@mintplayer/ng-router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ApplicationManager } from './states/application/application.manager';
import { ExternalLoginComponent } from './components/external-login/external-login.component';

@NgModule({
	declarations: [
		AppComponent,
  ExternalLoginComponent
	],
	imports: [
		BrowserModule,
		BrowserAnimationsModule,
		HttpClientModule,
		HttpClientXsrfModule.withOptions({
			cookieName: 'XSRF-TOKEN',
			headerName: 'X-XSRF-TOKEN'
		}),
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
