import Component from '@ember/component';

import { tagName } from '@ember-decorators/component';

// @ts-ignore -- need to generate style modules
import style from './style';

@tagName('')
export default class Loader extends Component {
  style = style;
}