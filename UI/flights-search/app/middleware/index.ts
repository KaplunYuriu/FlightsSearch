import thunk from 'redux-thunk';
import AirportsService from 'flights-search/services/airports';

import arrayMiddleWare from 'flights-search/middleware/array';
import promiesMiddleWare from 'flights-search/middleware/promise';
import RoutesService from 'flights-search/services/routes';
import CitiesService from 'flights-search/services/locations';

const thunkMiddleware = thunk.withExtraArgument({
  airportsService: new AirportsService(),
  routesService: new RoutesService(),
  citiesService: new CitiesService()
});

export default [arrayMiddleWare, promiesMiddleWare, thunkMiddleware];
