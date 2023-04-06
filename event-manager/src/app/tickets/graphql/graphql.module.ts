import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ApolloModule, APOLLO_OPTIONS } from 'apollo-angular';
import { HttpLink } from 'apollo-angular/http';
import { ApolloClientOptions, InMemoryCache, split } from '@apollo/client/core';
import { HttpClientModule } from '@angular/common/http';
import { createPersistedQueryLink } from 'apollo-angular/persisted-queries';
import { sha256 } from 'crypto-hash';

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
          uri: 'https://localhost:5001/graphql',
          method: 'GET'
        });

        const link = http;
        /*const link = createPersistedQueryLink({
          sha256,
          useGETForHashedQueries: true
        }
        ).concat(http);
*/
        return {
          cache: new InMemoryCache(),
          link: link,
        };
      },
      deps: [HttpLink],
    },
  ]
})
export class GraphqlModule { }
