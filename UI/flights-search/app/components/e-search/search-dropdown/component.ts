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
  updateHandler: any;
  disabled: boolean;
  selectedAirport: Airport;

  @computed('selectedAirport')
  get isDisabled() {
    return _.isUndefined(this.selectedAirport);
  }

  @action
  updateAirport(airport) {
    this.updateHandler(airport);
  }
}