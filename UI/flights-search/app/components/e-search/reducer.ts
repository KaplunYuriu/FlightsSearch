import { AsyncActionStatus } from "flights-search/types";

export enum ActionTypes {
  LoadAirports = 'LoadAirports'
}

export function createAction(type: ActionTypes, payload: any): Action {
  return { type, payload, status: AsyncActionStatus.Pending };
}

export function loadAirports() {
  return (dispatch, getState, { airportService }) =>
    dispatch(createAction(ActionTypes.LoadAirports, airportService.getAirports()));
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

export interface Action {
  readonly status: AsyncActionStatus;
  readonly type: ActionTypes;
  readonly payload?: any;
}

export default function (
  state: AirportsState = initialState,
  action: Action
): AirportsState {
  switch (action.type) {
    case ActionTypes.LoadAirports:
      if (action.status === AsyncActionStatus.Successful) {
        return Object.assign({}, state, {
          airports: action.payload,
          isEarlyFireLoading: false,
        }) as AirportsState;
      }

    default:
      return state;
  }
}
