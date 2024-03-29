import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IsLoggedInGuard } from '../../guards/is-logged-in/is-logged-in.guard';

const routes: Routes = [
	{ path: 'login', loadChildren: () => import('./login/login.module').then(m => m.LoginModule) },
	{ path: 'register', loadChildren: () => import('./register/register.module').then(m => m.RegisterModule) },
	{ path: 'profile', loadChildren: () => import('./profile/profile.module').then(m => m.ProfileModule), canActivate: [IsLoggedInGuard] },
//#if (UseTwoFactorAuthentication)
	{ path: 'two-factor', loadChildren: () => import('./two-factor/two-factor.module').then(m => m.TwoFactorModule) },
//#endif
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class AccountRoutingModule { }
