import { AsyncActionStatus } from "flights-search/types";
import _ from "lodash";

export enum ActionTypes {
  SearchAirports = 'SearchAirports',
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
}

export type Route = {
  departure: Airport;
  destination: Airport;
  airline: string;
  isActive: boolean;
}

export function searchAirports(pattern: string) {
  return (dispatch, getState, { airportsService }) => {
    return dispatch({
      type: ActionTypes.SearchAirports,
      payload: airportsService.searchAirports(pattern)
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
  sourceAirports: Airport[];
  departureAirports: Airport[],
  destinationAirports: Airport[],

  routes: Route[],

  selectedDepartureAirport: Airport,
  selectedDestinationAirport: Airport
};

const initialState: AirportsState = {
  sourceAirports: [],
  departureAirports: [],
  destinationAirports: [],

  routes: [],

  selectedDepartureAirport: undefined,
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
          sourceAirports: action.payload,
          departureAirports: action.payload
        }) as AirportsState;
      }

    case ActionTypes.UpdateDepartureAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload !== state.selectedDepartureAirport ? undefined : state.selectedDestinationAirport,
        selectedDepartureAirport: action.payload,
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
