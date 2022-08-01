import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedInGuard } from '../../guards/is-logged-in/is-logged-in.guard';

const routes: Routes = [{
	path: 'weatherforecasts',
	loadChildren: () => import('./weatherforecasts/weatherforecasts.module').then(m => m.WeatherforecastsModule),
	canActivate: [IsLoggedInGuard]
}];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class InformationRoutingModule { }
