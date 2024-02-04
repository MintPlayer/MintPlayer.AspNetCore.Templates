import 'zone.js/node';
import 'reflect-metadata';
import { renderApplication } from '@angular/platform-server';
import { enableProdMode, StaticProvider } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { bootstrapApplication } from '@angular/platform-browser';
import { createServerRenderer } from 'aspnet-prerendering';
import { DATA_FROM_SERVER } from './app/providers/data-from-server';
import { AppComponent } from './app/app.component';
import { config as serverConfig } from './app/app.config.server';

enableProdMode();

export default createServerRenderer(params => {
  const providers: StaticProvider[] = [
    { provide: APP_BASE_HREF, useValue: params.baseUrl },
    { provide: DATA_FROM_SERVER, useValue: params.data },
  ];

  // Bypass ssr api call cert warnings in development
  process.env['NODE_TLS_REJECT_UNAUTHORIZED'] = "0";

  const renderPromise = renderApplication(() => bootstrapApplication(AppComponent, serverConfig), {
    document: params.data.originalHtml,
    url: params.url,
    platformProviders: providers,
  });

  return renderPromise.then(html => ({ html }));
});
