import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WeatherforecastsComponent } from './weatherforecasts.component';

const routes: Routes = [{ path: '', component: WeatherforecastsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WeatherforecastsRoutingModule { }
