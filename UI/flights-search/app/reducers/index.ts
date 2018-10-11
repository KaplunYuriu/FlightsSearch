import { combineReducers } from 'redux';
import airports from 'flights-search/components/e-search/reducer';
import global from 'flights-search/components/e-application/reducer';

export default combineReducers({
  airports,
  global
});
