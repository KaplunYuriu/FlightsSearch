import Component from '@ember/component';
import hbs from 'htmlbars-inline-precompile';
import { tagName } from '@ember-decorators/component';
import { connect } from 'ember-redux';
// @ts-ignore -- need to generate style modules
import style from './style';
import { updateDepartureAirport, updateDestinationAirport } from 'flights-search/components/e-search/reducer';

const stateToComputed = state => {
  const {
    departureAirports,
    destinationAirports,
    selectedDepartureAirport,
    selectedDestinationAirport
  } = state.airports;

  return {
    departureAirports,
    destinationAirports,
    selectedDepartureAirport,
    selectedDestinationAirport
  };
};

const dispatchToActions = {
  updateDepartureAirport,
  updateDestinationAirport
};


@tagName('')
class SearchContainer extends Component {
  style = style;
  layout = hbs`{{yield (hash
    style=style
    departureAirports=departureAirports    
    destinationAirports=destinationAirports
    selectedDepartureAirport=selectedDepartureAirport
    selectedDestinationAirport=selectedDestinationAirport
    updateDepartureAirport=(action "updateDepartureAirport")
    updateDestinationAirport=(action "updateDestinationAirport")
  )}}`;
}

export default connect(stateToComputed, dispatchToActions)(SearchContainer);