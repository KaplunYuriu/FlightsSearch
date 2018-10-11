import fetch, { handleRejection } from 'flights-search/utils/fetch';

export default class AirportsService {
  getAirports() {
    return fetch(`/airports`).catch(handleRejection);
  }
}