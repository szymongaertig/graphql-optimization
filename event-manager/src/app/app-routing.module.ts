import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventsWithEmailsComponent } from './events-with-emails/events-with-emails.component';

const routes: Routes = [
  {
    path: 'events-with-emails',
    component: EventsWithEmailsComponent
  },
  {
    path: 'tickets',
    loadChildren: () => import('./tickets/tickets.module').then(m=> m.TicketsModule)
  },
  {
    path: 'registrations',
    loadChildren: () => import('./registrations/registrations.module').then(m=> m.RegistrationsModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

}
