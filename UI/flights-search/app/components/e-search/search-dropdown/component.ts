import Component from '@ember/component';

import { tagName } from '@ember-decorators/component';
import { action, computed } from '@ember-decorators/object';

// @ts-ignore -- need to generate style modules
import style from './style';
import { Airport } from 'flights-search/components/e-search/reducer';
import _ from 'lodash';

@tagName('')
export default class SearchDropdown extends Component {
  style = style;
  airports: Airport[];

  _airport: Airport;

  didReceiveAttrs() {
    this.set('_airport', this.airports[0]);
  }

  @action
  updateAirport(airport) {
    this.set('_airport', airport);
  }
}