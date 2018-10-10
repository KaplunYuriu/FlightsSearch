import Component from '@ember/component';

import { tagName } from '@ember-decorators/component';

// @ts-ignore -- need to generate style modules
import style from './style';
import { Airport } from 'flights-search/components/e-search/reducer';

@tagName('')
export default class SearchDropdown extends Component {
  style = style;
  airports: Airport[];
}