import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
	{ path: 'weatherforecasts', loadChildren: () => import('./weatherforecasts/weatherforecasts.module').then(m => m.WeatherforecastsModule) }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class InformationRoutingModule { }
