import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
	{ path: '', loadChildren: () => import('./home/home.module').then(m => m.HomeModule) },
	{ path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule) }
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class PagesRoutingModule { }
