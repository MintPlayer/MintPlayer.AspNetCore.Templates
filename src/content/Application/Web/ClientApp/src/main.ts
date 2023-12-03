import { enableProdMode, StaticProvider } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

////#if (UseNgxTranslate)
import { AppBrowserModule } from './app/app.browser.module';
////#else
import { AppModule } from './app/app.module';
////#endif
////#if (UseServerSideRendering)
import { DATA_FROM_SERVER } from './app/providers/data-from-server';
////#endif
import { environment } from './environments/environment';

if (environment.production) {
	enableProdMode();
}

const providers: StaticProvider[] = [
////#if (UseServerSideRendering)
	{ provide: DATA_FROM_SERVER, useValue: null },
////#endif
];

function bootstrap() {
////#if (UseNgxTranslate)
	platformBrowserDynamic(providers).bootstrapModule(AppBrowserModule)
		.catch(err => console.error(err));
////#else
	platformBrowserDynamic(providers).bootstrapModule(AppModule)
		.catch(err => console.error(err));
////#endif
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
