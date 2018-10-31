import fetch, { handleRejection } from 'flights-search/utils/fetch';
import { Airport } from 'flights-search/components/e-search/reducer';

export default class RouteService {
  getRoutesBetween(departure: Airport, destination: Airport) {
    return fetch(`/routes/between?departure=${departure.alias}&destination=${destination.alias}`).catch(handleRejection);
  }
}