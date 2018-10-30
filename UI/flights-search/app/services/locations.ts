import fetch, { handleRejection } from 'flights-search/utils/fetch';

export default class CitiesService {
  searchLocations(pattern: string) {
    return fetch(`/cities/search?pattern=${pattern}`).catch(handleRejection);
  }
}