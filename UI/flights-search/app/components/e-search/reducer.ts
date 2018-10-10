import { AsyncActionStatus } from "flights-search/types";

export enum ActionTypes {
  LoadAirports = 'LoadAirports'
}

export function loadAirports() {
  return (dispatch, getState, { airportService }) => {
    return dispatch({
      type: ActionTypes.LoadAirports,
      payload: airportService.getAirports(),
    });
  }
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

export type AirportsState = {
  airports: Airport[];
};

const initialState: AirportsState = {
  airports: []
};

export default function (
  state: AirportsState = initialState,
  action
): AirportsState {
  switch (action.type) {
    case ActionTypes.LoadAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          airports: action.payload
        }) as AirportsState;
      }
      return state;

    default:
      return state;
  }
}
