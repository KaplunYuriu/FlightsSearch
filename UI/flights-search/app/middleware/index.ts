import thunk from 'redux-thunk';
import AirportsService from 'flights-search/services/airports';

import arrayMiddleWare from 'flights-search/middleware/array';
import promiesMiddleWare from 'flights-search/middleware/promise';
import RoutesService from 'flights-search/services/routes';

const thunkMiddleware = thunk.withExtraArgument({
  airportsService: new AirportsService(),
  routesService: new RoutesService()
});

export default [arrayMiddleWare, promiesMiddleWare, thunkMiddleware];
