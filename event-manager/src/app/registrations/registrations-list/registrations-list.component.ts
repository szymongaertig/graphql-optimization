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

  public displayedColumns = ['id', 'registrationDate', 'checkInDate', 'status'];

  private GetRegistrationsQuery = gql`
  query registrations{
    registrations:eventRegistrations{
      id
      name
      surname
      registrationDate
      emailAddress
      checkInDate
      status
    }
  }
  `;

  ngOnInit(): void {
    this.loading = false;
    this.querySubscription = this.apollo.watchQuery<GetRegistrationsQueryResult>({
      query: this.GetRegistrationsQuery,
      fetchPolicy: 'cache-first'
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
