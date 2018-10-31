import Component from '@ember/component';
import hbs from 'htmlbars-inline-precompile';
import { tagName } from '@ember-decorators/component';
import { connect } from 'ember-redux';
// @ts-ignore -- need to generate style modules
import style from './style';
import { updateDepartureAirport, updateDestinationAirport, searchAirports, clearState, searchLocations, updateDepartureLocation, searchForRoutes } from 'flights-search/components/e-search/reducer';

const stateToComputed = state => {
  const {
    closestAirports,
    locations,
    selectedDepartureLocation,
    airports,
    selectedDepartureAirport,
    selectedDestinationAirport,
    routes,
  } = state.airports;

  return {
    closestAirports,
    locations,
    selectedDepartureLocation,
    airports,
    selectedDepartureAirport,
    selectedDestinationAirport,
    routes
  };
};

const dispatchToActions = {
  searchAirports,
  updateDepartureAirport,
  updateDestinationAirport,
  clearState,
  searchLocations,
  updateDepartureLocation,
  searchForRoutes
};


@tagName('')
class SearchContainer extends Component {
  style = style;
  layout = hbs`{{yield (hash
    style=style
    closestAirports=closestAirports
    locations=locations
    selectedDepartureLocation=selectedDepartureLocation
    airports=airports    
    selectedDepartureAirport=selectedDepartureAirport
    selectedDestinationAirport=selectedDestinationAirport
    routes=routes
    searchAirports=(action "searchAirports")
    updateDepartureAirport=(action "updateDepartureAirport")
    updateDestinationAirport=(action "updateDestinationAirport")
    clearState=(action "clearState")
    searchLocations=(action "searchLocations")
    updateDepartureLocation=(action "updateDepartureLocation")
    searchForRoutes=(action "searchForRoutes")
  )}}`;
}

export default connect(stateToComputed, dispatchToActions)(SearchContainer);