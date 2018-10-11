import fetch, { handleRejection } from 'flights-search/utils/fetch';
import { Airport } from 'flights-search/components/e-search/reducer';

export default class RouteService {
  getAvailableDestinations(airport: Airport) {
    return fetch(`/routes/${airport.alias}/outgoing`).catch(handleRejection);
  }
}