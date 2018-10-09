import Route from '@ember/routing/route';

export default class Index extends Route {
  renderTemplate(controller, model) {
    const orderController = this.controllerFor('search');

    return this.render('search', {
      outlet: 'search',
      controller,
    });
  }
}
