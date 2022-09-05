import { enableProdMode, StaticProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

if (environment.production) {
	enableProdMode();
}

const providers: StaticProvider[] = [
];

function bootstrap() {
	platformBrowserDynamic(providers).bootstrapModule(AppModule)
		.catch(err => console.error(err));
};

////#if (UseServerSideRendering)
if (document.readyState === 'complete') {
	bootstrap();
} else {
	document.addEventListener('DOMContentLoaded', bootstrap);
}
////#else
bootstrap();
////#endif
