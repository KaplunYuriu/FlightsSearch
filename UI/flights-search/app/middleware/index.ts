import thunk from 'redux-thunk';
import AirportService from 'flights-search/services/airport';

import arrayMiddleWare from 'flights-search/middleware/array';
import promiesMiddleWare from 'flights-search/middleware/promise';

const thunkMiddleware = thunk.withExtraArgument({
  airportService: new AirportService()
});

export default [arrayMiddleWare, promiesMiddleWare, thunkMiddleware];
