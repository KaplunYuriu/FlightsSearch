import fetch, { handleRejection } from 'flights-search/utils/fetch';

export default class AirportsService {
  searchAirports(pattern: string) {
    return fetch(`/airports/search?pattern=${pattern}`).catch(handleRejection);
  }

  findClosestAirports(cityId: number) {
    return fetch(`/airports/closesttocity?cityId=${cityId}`).catch(handleRejection);
  }
}