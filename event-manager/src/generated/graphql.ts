import { gql } from 'apollo-angular';
export type Maybe<T> = T | null;
export type InputMaybe<T> = Maybe<T>;
export type Exact<T extends { [key: string]: unknown }> = { [K in keyof T]: T[K] };
export type MakeOptional<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]?: Maybe<T[SubKey]> };
export type MakeMaybe<T, K extends keyof T> = Omit<T, K> & { [SubKey in K]: Maybe<T[SubKey]> };
/** All built-in and custom scalars, mapped to their actual values */
export type Scalars = {
  ID: string;
  String: string;
  Boolean: boolean;
  Int: number;
  Float: number;
  /** The `DateTime` scalar represents an ISO-8601 compliant date time type. */
  DateTime: any;
  UUID: any;
};

/** The scope of a cache hint. */
export enum CacheControlScope {
  /** The value to cache is specific to a single user. */
  Private = 'PRIVATE',
  /** The value to cache is not tied to a single user. */
  Public = 'PUBLIC'
}

export type Client = {
  __typename?: 'Client';
  emailAddress: Scalars['String'];
  name: Scalars['String'];
  surname: Scalars['String'];
};

export type Email = {
  __typename?: 'Email';
  body: Scalars['String'];
  creationDate: Scalars['DateTime'];
  sentDate?: Maybe<Scalars['DateTime']>;
  subject: Scalars['String'];
};

export type Mutation = {
  __typename?: 'Mutation';
  checkIn?: Maybe<Registration>;
  createRandomRegistration?: Maybe<Registration>;
};


export type MutationCheckInArgs = {
  registrationId: Scalars['UUID'];
};

export type Query = {
  __typename?: 'Query';
  eventRegistrations: Array<Registration>;
  ticket: Ticket;
};


export type QueryTicketArgs = {
  ticketId: Scalars['UUID'];
};

export type Registration = {
  __typename?: 'Registration';
  checkInDate?: Maybe<Scalars['DateTime']>;
  client?: Maybe<Client>;
  clientId: Scalars['UUID'];
  emails?: Maybe<Array<Maybe<Email>>>;
  emailsStream?: Maybe<Array<Maybe<Email>>>;
  emailsWithLoader?: Maybe<Array<Maybe<Email>>>;
  eventId: Scalars['UUID'];
  id: Scalars['UUID'];
  registrationDate: Scalars['DateTime'];
  status: RegistrationStatus;
};

export enum RegistrationStatus {
  Completed = 'COMPLETED',
  WaitingForPayment = 'WAITING_FOR_PAYMENT'
}

export type Subscription = {
  __typename?: 'Subscription';
  onRegistered: Registration;
  onRegistrationUpdated: Registration;
};

export type Ticket = {
  __typename?: 'Ticket';
  client?: Maybe<Client>;
  clientId: Scalars['UUID'];
  id: Scalars['UUID'];
  registrationDate: Scalars['DateTime'];
  status: RegistrationStatus;
};
