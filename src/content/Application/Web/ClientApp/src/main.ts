import { enableProdMode, StaticProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
////#if (SsrOnSupplyData)
import { DATA_FROM_SERVER } from './app/providers/data-from-server';
////#endif
import { environment } from './environments/environment';

if (environment.production) {
	enableProdMode();
}

const providers: StaticProvider[] = [
////#if (SsrOnSupplyData)
	{ provide: DATA_FROM_SERVER, useValue: null },
////#endif
];

function bootstrap() {
	platformBrowserDynamic(providers).bootstrapModule(AppModule)
		.catch(err => console.error(err));
};

////#if (SsrOnSupplyData)
if (document.readyState === 'complete') {
	bootstrap();
} else {
	document.addEventListener('DOMContentLoaded', bootstrap);
}
////#else
bootstrap();
////#endif
