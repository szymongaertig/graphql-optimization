import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GraphqlModule } from './graphql/graphql.module';
import { TicketComponent } from './ticket/ticket.component';
import { TicketRoutingModule } from './ticket-routing/ticket-routing.module';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCardModule } from '@angular/material/card';


@NgModule({
  declarations: [
    TicketComponent
  ],
  imports: [
    CommonModule,
    GraphqlModule,
    TicketRoutingModule,
    MatProgressBarModule,
    MatCardModule
  ]
})
export class TicketsModule { }
