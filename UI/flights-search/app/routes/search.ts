import Route from '@ember/routing/route';
import { service } from '@ember-decorators/service';
import _ from 'lodash';
import { loadAirports } from 'flights-search/components/e-search/reducer';

export default class Search extends Route {
  @service redux;

  renderTemplate(controller, model) {
    const state = this.redux.getState();
    const dataMustBeRefreshed = _.isEmpty(state.airports.airports);

    if (dataMustBeRefreshed) {
      return this.redux.dispatch(loadAirports()).then(() => {
        return this.render('search', {
          outlet: 'search',
          controller,
        })
      });
    }
  }
}
