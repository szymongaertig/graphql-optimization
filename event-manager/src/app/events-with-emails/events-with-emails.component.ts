import { Component, OnInit } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';

@Component({
  selector: 'app-events-with-emails',
  templateUrl: './events-with-emails.component.html',
  styleUrls: ['./events-with-emails.component.css']
})
export class EventsWithEmailsComponent implements OnInit {

  constructor(private apollo:Apollo) {

  }

  ngOnInit(): void {
   this.apollo.subscribe({
    query: gql`
    subscription {
      onRegistered {
        id
        registrationDate
        name
        surname
      }
    }
    `
   }).subscribe(result =>{
    console.info(JSON.stringify(result))
   })
  }
}
