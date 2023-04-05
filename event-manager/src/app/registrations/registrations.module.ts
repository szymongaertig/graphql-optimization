import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegistrationsListComponent } from './registrations-list/registrations-list.component';
import { RegistrationRoutingModule } from './registration-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { RegistrationsGraphqlModule } from './registrations-graphql.module';
import {MatTableModule} from '@angular/material/table';
import { MatProgressBarModule} from '@angular/material/progress-bar';


@NgModule({
  declarations: [
    RegistrationsListComponent
  ],
  imports: [
    CommonModule,
    RegistrationRoutingModule,
    HttpClientModule,
    RegistrationsGraphqlModule,
    MatTableModule,
    MatProgressBarModule
  ]
})
export class RegistrationsModule { }
