import fetch, { handleRejection } from 'flights-search/utils/fetch';

export default class AirportService {
  getAirports() {
    return fetch(`/airports`).catch(handleRejection);
  }
}