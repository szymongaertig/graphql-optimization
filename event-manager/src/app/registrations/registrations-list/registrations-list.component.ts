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
      //pollInterval: 1000
    }).valueChanges
      .subscribe(({ data, loading }) => {
        this.loading = loading;
        this.registrations = data.registrations;
      }, error => {
        // todo: handle
      });

      //this.subscribeOnRegistered();
      this.subscribeOnUpdated();
  }

  private subscribeOnUpdated(){
    this.apollo.subscribe({
      query: gql`subscription {
        onRegistrationUpdated {
          __typename
          id
          checkInDate
        }
      }`
    }).subscribe(result =>{

    });
  }
  private subscribeOnRegistered(){
    this.apollo.subscribe<OnRegisteredSubscriptionResult>({
      query: gql`subscription {
        registration:onRegistered {
          id
          name
          surname
          checkInDate
          registrationDate
          emailAddress
          status
        }
      }`
    }).subscribe(result => {
      const existingRegistrations = this.apollo.client.readQuery<GetRegistrationsQueryResult>({
        query: this.GetRegistrationsQuery
      });

      this.apollo.client.writeQuery({
        query: this.GetRegistrationsQuery,
        data: {
          registrations: [result.data?.registration!, ...existingRegistrations?.registrations!]
        }
      });

    })
  }

  public checkIn(registration: Registration){
    console.info(registration.id);
    this.apollo.mutate({
      mutation: gql`
      mutation checkIn($registrationId: UUID!){
        registration:checkIn(registrationId: $registrationId) {
          id
          checkInDate
        }
      }`,
      variables: {
        registrationId: registration.id
      },
      optimisticResponse: {
        registration : {
          __typename: "Registration",
          id: registration.id,
          checkInDate: new Date()
        }
      }
    }).subscribe(result =>{

    });
  }
}
