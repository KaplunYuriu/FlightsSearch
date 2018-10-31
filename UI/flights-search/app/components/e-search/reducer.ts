import { AsyncActionStatus } from "flights-search/types";
import _ from "lodash";

export enum ActionTypes {
  SearchAirports = 'SearchAirports',
  SearchLocations = 'SearchLocations',
  UpdateDepartureLocation = 'UpdateDepartureLocation',
  UpdateClosestAirports = 'UpdateClosestAirports',
  UpdateDepartureAirport = 'UpdateDepartureAirport',
  UpdateDestinationAirport = 'UpdateDestinationAirport',
  SearchForRoutes = 'SearchForRoutes',
  ClearState = 'ClearState'
}

export type Airport = {
  name: string;
  alias: string;
  city: string;
  country: string;
  latitude: string;
  longitude: string;
  altitude: string;
  displayName: string;
}

export type Route = {
  departure: Airport;
  destination: Airport;
  airline: string;
  isActive: boolean;
}

export type Location = {
  id: number;
  city: string;
}

export function searchAirports(pattern: string) {
  return (dispatch, getState, { airportsService }) => {
    return dispatch({
      type: ActionTypes.SearchAirports,
      payload: airportsService.searchAirports(pattern)
    });
  }
}

export function searchLocations(pattern: string) {
  return (dispatch, getState, { citiesService }) => {
    return dispatch({
      type: ActionTypes.SearchLocations,
      payload: citiesService.searchLocations(pattern)
    });
  }
}

export function updateDepartureLocation(location: Location) {
  return (dispatch, getState, { airportsService }) => {
    dispatch({
      type: ActionTypes.UpdateDepartureLocation,
      payload: location
    });

    return dispatch({
      type: ActionTypes.UpdateClosestAirports,
      payload: airportsService.findClosestAirports(location.id)
    });
  }
}

export function updateDepartureAirport(airport: Airport) {
  return (dispatch, getState, { routesService }) => {
    return dispatch({
      type: ActionTypes.UpdateDepartureAirport,
      payload: airport
    });
  };
}

export function searchForRoutes() {
  return (dispatch, getState, { routesService }) => {
    var departureAirport = getState().airports.selectedDepartureAirport;
    var destinationAirport = getState().airports.selectedDestinationAirport;

    if (_.isNil(destinationAirport)) {
      return dispatch({
        type: ActionTypes.SearchForRoutes,
        payload: routesService.getAvailableDestinations(departureAirport).then(resp => {
          return mapToRoutes(resp);
        })
      });
    }
    return dispatch({
      type: ActionTypes.SearchForRoutes,
      payload: routesService.getRoutesBetween(departureAirport, destinationAirport).then(resp => {
        return mapToRoutes(resp);
      })
    });
  }
}

export function updateDestinationAirport(airport: Airport) {
  return (dispatch, getState, { routesService }) => {
    return dispatch({
      type: ActionTypes.UpdateDestinationAirport,
      payload: airport
    })
  };
}

export function clearState() {
  return (dispatch, getState, { routesService }) => {
    return dispatch({
      type: ActionTypes.ClearState,
      payload: undefined
    })
  };
}

function mapToRoutes(response): Route[] {
  return response.map(function (route) {
    return {
      departure: route.departure,
      destination: route.destination,
      airline: route.airline.name,
      isActive: route.airline.active
    }
  });
}

export type AirportsState = {
  closestAirports: Map<number, Airport>;
  airports: Airport[],
  locations: Location[],

  routes: Route[],

  selectedDepartureAirport: Airport,
  selectedDepartureLocation: Location,
  selectedDestinationAirport: Airport
};

const initialState: AirportsState = {
  closestAirports: undefined,
  airports: [],
  locations: [],

  routes: undefined,

  selectedDepartureAirport: undefined,
  selectedDepartureLocation: undefined,
  selectedDestinationAirport: undefined
};

export default function (
  state: AirportsState = initialState,
  action
): AirportsState {
  switch (action.type) {
    case ActionTypes.SearchAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          airports: action.payload
        }) as AirportsState;
      }
      break;
    
    case ActionTypes.SearchLocations:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          locations: action.payload
        }) as AirportsState;
      }
      break;
    
    case ActionTypes.UpdateDepartureLocation:
      return Object.assign({}, state, {
        selectedDepartureLocation: action.payload
      });
      

    case ActionTypes.UpdateClosestAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          closestAirports: action.payload
        }) as AirportsState;
      }
      break;

    case ActionTypes.UpdateDepartureAirport:
      return Object.assign({}, state, {
        selectedDepartureAirport: action.payload,
        locations: [],
        routes: []
      });

    case ActionTypes.UpdateDestinationAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload
      });

    case ActionTypes.SearchForRoutes:
      return Object.assign({}, state, {
        routes: action.payload
      });

    case ActionTypes.ClearState: 
      return Object.assign({}, state, initialState);

    default:
      return state;
  }

  return state;
}
