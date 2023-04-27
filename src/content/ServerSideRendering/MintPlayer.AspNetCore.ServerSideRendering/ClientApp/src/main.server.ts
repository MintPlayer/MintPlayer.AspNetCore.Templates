/***************************************************************************************************
 * Initialize the server environment - for example, adding DOM built-in types to the global scope.
 *
 * NOTE:
 * This import must come before any imports (direct or transitive) that rely on DOM built-ins being
 * available, such as `@angular/elements`.
 */
import '@angular/platform-server/init';
import 'zone.js/node';
import 'reflect-metadata';
import { renderModule } from '@angular/platform-server';
import { APP_BASE_HREF } from '@angular/common';
import { enableProdMode, StaticProvider } from '@angular/core';
import { createServerRenderer } from 'aspnet-prerendering';
import { BOOT_FUNC_PARAMS } from '@mintplayer/ng-base-url';
import { DATA_FROM_SERVER } from './app/providers/data-from-server';
import { AppServerModule } from './app/app.server.module';

enableProdMode();

export { AppServerModule } from './app/app.server.module';
export default createServerRenderer(params => {
	const providers: StaticProvider[] = [
		{ provide: BOOT_FUNC_PARAMS, useValue: params },
		{ provide: DATA_FROM_SERVER, useValue: params.data }
	];

	const options = {
		document: params.data.originalHtml,
		url: params.url,
		extraProviders: providers
	};

	// Bypass ssr api call cert warnings in development
	process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = "0";

	const renderPromise = renderModule(AppServerModule, options);

	return renderPromise.then(html => ({ html }));
});
