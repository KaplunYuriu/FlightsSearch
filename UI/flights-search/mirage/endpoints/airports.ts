import Mirage, { faker } from 'ember-cli-mirage';

export default class Order extends Mirage.Factory.extend({
  name() {
    return faker.company.companyName();
  },
  alias() {
    return faker.company.companySuffix();
  },
  city() {
    return faker.address.city();
  },
  country() {
    return faker.address.country();
  },
  latitude() {
    return faker.address.latitude();
  },
  longitude() {
    return faker.address.longitude();
  },
  altitude() {
    return faker.random.number({ min: 100, max: 2500 });
  }
}) { };