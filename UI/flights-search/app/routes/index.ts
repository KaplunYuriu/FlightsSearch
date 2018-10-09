import Route from '@ember/routing/route';

export default class Index extends Route {
  activate() {
    super.activate();
    this.transitionTo('search');
  }
}
