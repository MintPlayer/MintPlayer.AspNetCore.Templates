import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
  { path: 'counter', loadChildren: () => import('./counter/counter.module').then(m => m.CounterModule) },
  { path: 'fetch-data', loadChildren: () => import('./fetch-data/fetch-data.module').then(m => m.FetchDataModule) },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PagesRoutingModule { }
