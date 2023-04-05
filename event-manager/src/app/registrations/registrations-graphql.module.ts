import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApolloModule, APOLLO_OPTIONS } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { InMemoryCache, split } from '@apollo/client/core';
import { HttpClientModule } from '@angular/common/http';
import { WebSocketLink } from '@apollo/client/link/ws';
import { getMainDefinition } from '@apollo/client/utilities';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule,
    ApolloModule
  ],
  providers: [
    {
      provide: APOLLO_OPTIONS,
      useFactory(httpLink: HttpLink) {
        const http = httpLink.create({
          uri: 'https://localhost:5001/graphql'
        });

        const ws = new WebSocketLink({
          uri: 'wss://localhost:5001/graphql',
          options: {
            reconnect: true,
          },
        });

        const linkWithWss = split(
          // split based on operation type
          ({ query }) => {
            const definition = getMainDefinition(query);
            return definition.kind === 'OperationDefinition'
            && definition.operation === 'subscription';
          },
          ws,
          http,
        );

        const simpleLink = http;

        return {
          cache: new InMemoryCache(),
          link: linkWithWss,
        };
      },
      deps: [HttpLink],
    },
  ]
})
export class RegistrationsGraphqlModule { }
