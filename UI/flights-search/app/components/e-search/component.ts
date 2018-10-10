import Component from '@ember/component';
import hbs from 'htmlbars-inline-precompile';
import { tagName } from '@ember-decorators/component';
import { connect } from 'ember-redux';
// @ts-ignore -- need to generate style modules
import style from './style';
import { loadAirports } from 'flights-search/components/e-search/reducer';

const stateToComputed = state => {
  const {
    airports
  } = state.airports;

  return {
    airports
  };
};

const dispatchToActions = {
  loadAirports
};


@tagName('')
class SearchContainer extends Component {
  style = style;
  layout = hbs`{{yield (hash
    style=style
    airports=airports
    loadAirports=loadAirports
  )}}`;
}

export default connect(stateToComputed, dispatchToActions)(SearchContainer);