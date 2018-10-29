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
  searchHandler: any;
  disabled: boolean;
  selectedAirport: Airport;

  @computed('airports')
  get isDisabled() {
    return this.disabled;
  }

  @computed('isDisabled')
  get placeholder() {
    if (this.disabled)
      return undefined;

    return "Start typing...";
  }

  @action
  search(query) {
    if (query.length < 3)
      return;

    this.searchHandler(query);
  }

  @action
  updateAirport(airport) {
    this.updateHandler(airport);
  }
}