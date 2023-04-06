import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Apollo, gql } from 'apollo-angular';
import { Ticket } from 'src/generated/graphql';

export class GetTicketResult {
  constructor(public ticket: Ticket) {

  }
}
@Component({
  selector: 'app-ticket',
  templateUrl: './ticket.component.html',
  styleUrls: ['./ticket.component.css']
})
export class TicketComponent implements OnInit {

  public loading: boolean = false;
  public ticket? : Ticket;
  constructor(private route: ActivatedRoute, private apollo: Apollo) { }

  ngOnInit(): void {
    this.loading = true;
    const ticketId = this.route.snapshot.paramMap.get('ticketId');
    this.apollo.watchQuery<GetTicketResult>({
      query: gql`
      query getTicket($ticketId:UUID!){
        ticket(ticketId: $ticketId) {
          __typename
          id
          client {
            name
            surname
            emailAddress
          }
          status
          registrationDate
        }
      }
      `,
      variables: {
        ticketId: ticketId
      }
    }).valueChanges
    .subscribe(result =>{
      this.loading = false;
      this.ticket = result.data.ticket;
    })
  }
}
