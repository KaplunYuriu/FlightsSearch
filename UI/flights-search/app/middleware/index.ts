import thunk from 'redux-thunk';
import AirportService from 'flights-search/services/airport';

const thunkMiddleware = thunk.withExtraArgument({
  airportService: new AirportService()
});

export default [thunkMiddleware];
