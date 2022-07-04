import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FetchDataRoutingModule } from './fetch-data-routing.module';
import { FetchDataComponent } from './fetch-data.component';


@NgModule({
  declarations: [
    FetchDataComponent
  ],
  imports: [
    CommonModule,
    FetchDataRoutingModule
  ]
})
export class FetchDataModule { }
