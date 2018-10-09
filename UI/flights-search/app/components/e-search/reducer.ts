export enum ActionTypes {
  LoadAirports = 'LoadAirports'
}

export function loadAirports() {
  return (dispatch, getState, { airportService }) =>
    dispatch({
      type: ActionTypes.LoadAirports,
      payload: airportService.getAirports(),
    });
}