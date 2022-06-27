import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PagesRoutingModule } from './pages-routing.module';
import { AccountModule } from './account/account.module';


@NgModule({
  imports: [
    CommonModule,
    PagesRoutingModule,
    AccountModule
  ]
})
export class PagesModule { }
