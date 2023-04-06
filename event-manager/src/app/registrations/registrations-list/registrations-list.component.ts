import { Component, OnDestroy, OnInit } from '@angular/core';
import { Apollo, gql } from 'apollo-angular';
import { Registration } from 'src/generated/graphql';

export class OnRegisteredSubscriptionResult {
  constructor(public registration: Registration) {
  }
}
export class GetRegistrationsQueryResult {
  constructor(public registrations: Registration[]) {
  }
}
@Component({
  selector: 'app-registrations-list',
  templateUrl: './registrations-list.component.html',
  styleUrls: ['./registrations-list.component.css']
})
export class RegistrationsListComponent implements OnInit, OnDestroy {

  private querySubscription: any;

  public loading: boolean = false;
  public registrations: Registration[] = [];
  constructor(private apollo: Apollo) { }

  ngOnDestroy(): void {
    this.querySubscription.unsubscribe();
  }

  public displayedColumns =  ['name','registrationDate', 'checkInDate', 'status'];

  private GetRegistrationsQuery = gql`
  query registrations{
    registrations:eventRegistrations{
      id
      registrationDate
      checkInDate
      status

      client{
        name
        surname
        emailAddress
      }
    }
  }
  `;

  ngOnInit(): void {
    this.loading = false;
    this.querySubscription = this.apollo.watchQuery<GetRegistrationsQueryResult>({
      query: this.GetRegistrationsQuery,
      fetchPolicy: 'cache-first',
      pollInterval: 1000
    }).valueChanges
      .subscribe(({ data, loading }) => {
        this.loading = loading;
        this.registrations = data.registrations;
      }, error => {
        // todo: handle
      });

      //this.subscribeOnRegistered();
      //this.subscribeOnUpdated();
  }
}
