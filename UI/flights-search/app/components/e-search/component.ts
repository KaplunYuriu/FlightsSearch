import Component from '@ember/component';
import hbs from 'htmlbars-inline-precompile';
import { tagName } from '@ember-decorators/component';
import { connect } from 'ember-redux';
// @ts-ignore -- need to generate style modules
import style from './style';
import { updateDepartureAirport, updateDestinationAirport, searchAirports, clearDestinationAirport, searchLocations, updateDepartureLocation } from 'flights-search/components/e-search/reducer';

const stateToComputed = state => {
  const {
    availableAirports,
    locations,
    selectedDepartureLocation,
    departureAirports,
    destinationAirports,
    selectedDepartureAirport,
    selectedDestinationAirport,
    routes
  } = state.airports;

  return {
    availableAirports,
    locations,
    selectedDepartureLocation,
    departureAirports,
    destinationAirports,
    selectedDepartureAirport,
    selectedDestinationAirport,
    routes
  };
};

const dispatchToActions = {
  searchAirports,
  updateDepartureAirport,
  updateDestinationAirport,
  clearDestinationAirport,
  searchLocations,
  updateDepartureLocation
};


@tagName('')
class SearchContainer extends Component {
  style = style;
  layout = hbs`{{yield (hash
    style=style
    availableAirports=availableAirports
    locations=locations
    selectedDepartureLocation=selectedDepartureLocation
    departureAirports=departureAirports    
    destinationAirports=destinationAirports
    selectedDepartureAirport=selectedDepartureAirport
    selectedDestinationAirport=selectedDestinationAirport
    routes=routes
    searchAirports=(action "searchAirports")
    updateDepartureAirport=(action "updateDepartureAirport")
    updateDestinationAirport=(action "updateDestinationAirport")
    clearDestinationAirport=(action "clearDestinationAirport")
    searchLocations=(action "searchLocations")
    updateDepartureLocation=(action "updateDepartureLocation")
  )}}`;
}

export default connect(stateToComputed, dispatchToActions)(SearchContainer);