import Component from '@ember/component';

import { tagName } from '@ember-decorators/component';
import { action, computed } from '@ember-decorators/object';

// @ts-ignore -- need to generate style modules
import style from './style';
import _ from 'lodash';

@tagName('')
export default class SearchDropdown extends Component {
  style = style;
  options: any[];
  updateHandler: any;
  searchHandler: any;
  selectedOption: any;
  optionDisplayValue: any;

  get placeholder() {
    return "Start typing...";
  }

  @action
  displayValue(option) {
    return option.displayName;
  }

  @action
  search(query) {
    if (query.length < 3)
      return;

    this.searchHandler(query);
  }

  @action
  updateSelectedOption(option) {
    this.updateHandler(option);
  }
}