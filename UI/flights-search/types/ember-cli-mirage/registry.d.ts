import Airport from "flights-search/mirage/factories/airport";

export interface Models {
    'airport': Airport;
}

export type ModelNames = keyof Models;

export interface DbCollections {}

export interface ServerCollections {}

export type CollectionNames = keyof ServerCollections;