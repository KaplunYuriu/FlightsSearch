import { AsyncActionStatus } from "flights-search/types";
import _ from "lodash";

export enum ActionTypes {
  SearchAirports = 'SearchAirports',
  SearchLocations = 'SearchLocations',
  UpdateDepartureLocation = 'UpdateDepartureLocation',
  UpdateAvailableAirports = 'UpdateAvailableAirports',
  UpdateDepartureAirport = 'UpdateDepartureAirport',
  UpdateDestinationAirport = 'UpdateDestinationAirport',
  UpdateAvailableRoutes = 'UpdateAvailableRoutes',
  UpdateAvailableDestinationAirports = 'UpdateAvailableDestinationAirport'
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
      type: ActionTypes.UpdateAvailableAirports,
      payload: airportsService.findClosestAirports(location.id)
    });
  }
}

export function updateDepartureAirport(airport: Airport) {
  return (dispatch, getState, { routesService }) => {
    dispatch({
      type: ActionTypes.UpdateDepartureAirport,
      payload: airport
    });

    return dispatch({
      type: ActionTypes.UpdateAvailableDestinationAirports,
      payload: routesService.getAvailableDestinations(airport).then((resp) => {
        const destinationAirports = resp.map(function (route) { return route.destination });
        const availableRoutes = mapToRoutes(resp);

        dispatch(updateAvailableRoutes(availableRoutes));

        return destinationAirports;
      })
    });
  };
}

export function updateAvailableRoutes(routes: Route[]) {
  return dispatch => dispatch({
    type: ActionTypes.UpdateAvailableRoutes,
    payload: routes
  });
}

export function updateDestinationAirport(airport: Airport) {
  return (dispatch, getState, { routesService }) => {
    dispatch({
      type: ActionTypes.UpdateDestinationAirport,
      payload: airport
    })

    if (_.isUndefined(airport))
      return;

    return routesService.getRoutesBetween(getState().airports.selectedDepartureAirport, airport).then(resp => {
      const availableRoutes = mapToRoutes(resp);

      return dispatch(updateAvailableRoutes(availableRoutes));
    });
  };
}

export function clearDestinationAirport() {
  return (dispatch, getState, { routesService }) => {
    dispatch(updateDestinationAirport(undefined));
    return dispatch(updateDepartureAirport(getState().airports.selectedDepartureAirport));
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
  availableAirports: Map<number, Airport>;
  departureAirports: Airport[],
  destinationAirports: Airport[],
  locations: Location[],

  routes: Route[],

  selectedDepartureAirport: Airport,
  selectedDepartureLocation: Location,
  selectedDestinationAirport: Airport
};

const initialState: AirportsState = {
  availableAirports: undefined,
  departureAirports: [],
  destinationAirports: [],
  locations: [],

  routes: [],

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
          departureAirports: action.payload
        }) as AirportsState;
      }
    
    case ActionTypes.SearchLocations:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          locations: action.payload
        }) as AirportsState;
      }
    
    case ActionTypes.UpdateDepartureLocation:
      return Object.assign({}, state, {
        selectedDepartureLocation: action.payload,
        selectedDepartureAirport: undefined,
        destinationAirports: [],
        routes: []
      });

    case ActionTypes.UpdateAvailableAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          availableAirports: action.payload
        }) as AirportsState;
      }

    case ActionTypes.UpdateDepartureAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload !== state.selectedDepartureAirport ? undefined : state.selectedDestinationAirport,
        selectedDepartureAirport: action.payload,
        locations: [],
        routes: []
      });

    case ActionTypes.UpdateDestinationAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload
      });

    case ActionTypes.UpdateAvailableRoutes:
      return Object.assign({}, state, {
        routes: action.payload
      });

    case ActionTypes.UpdateAvailableDestinationAirports:
      return Object.assign({}, state, {
        destinationAirports: action.payload
      }) as AirportsState;

    default:
      return state;
  }
}
