import { enableProdMode, StaticProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { DATA_FROM_SERVER } from './app/providers/data-from-server';
import { environment } from './environments/environment';

if (environment.production) {
	enableProdMode();
}

function bootstrap() {
	const providers: StaticProvider[] = [
		{ provide: DATA_FROM_SERVER, useValue: null }
	];

	platformBrowserDynamic(providers).bootstrapModule(AppModule)
		.catch(err => console.error(err));
};


if (document.readyState === 'complete') {
	bootstrap();
} else {
	document.addEventListener('DOMContentLoaded', bootstrap);
}

