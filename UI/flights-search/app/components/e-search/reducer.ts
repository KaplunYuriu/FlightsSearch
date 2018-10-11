import { AsyncActionStatus } from "flights-search/types";
import _ from "lodash";

export enum ActionTypes {
  LoadAirports = 'LoadAirports',
  UpdateDepartureAirport = 'UpdateDepartureAirport',
  UpdateDestinationAirport = 'UpdateDestinationAirport',
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

export function loadAirports() {
  return (dispatch, getState, { airportsService }) => {
    return dispatch({
      type: ActionTypes.LoadAirports,
      payload: airportsService.getAirports()
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
        dispatch(updateDestinationAirport(resp.payload[0]));
      })
    });
  };
}

export function updateDestinationAirport(airport: Airport) {
  return dispatch => dispatch({
    type: ActionTypes.UpdateDestinationAirport,
    payload: airport
  });
}


export type AirportsState = {
  sourceAirports: Airport[];
  departureAirports: Airport[],
  destinationAirports: Airport[],

  selectedDepartureAirport: Airport,
  selectedDestinationAirport: Airport
};

const initialState: AirportsState = {
  sourceAirports: [],
  departureAirports: [],
  destinationAirports: [],

  selectedDepartureAirport: undefined,
  selectedDestinationAirport: undefined
};

export default function (
  state: AirportsState = initialState,
  action
): AirportsState {
  switch (action.type) {
    case ActionTypes.LoadAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          sourceAirports: action.payload,
          departureAirports: action.payload
        }) as AirportsState;
      }

    case ActionTypes.UpdateDepartureAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload !== state.selectedDepartureAirport ? undefined : state.selectedDestinationAirport,
        selectedDepartureAirport: action.payload
      });


    case ActionTypes.UpdateDestinationAirport:
      return Object.assign({}, state, {
        selectedDestinationAirport: action.payload
      });

    case ActionTypes.UpdateAvailableDestinationAirports:
      return Object.assign({}, state, {
        destinationAirports: action.payload
      }) as AirportsState;

    default:
      return state;
  }
}
