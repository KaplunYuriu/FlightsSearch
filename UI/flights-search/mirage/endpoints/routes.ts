import { Server } from 'ember-cli-mirage';
import _ from 'lodash';

export default function routesForRoutes(server: Server) {
  server.get('/routes/:alias/outgoing', ({ db }, request) => _.takeRight(db.airports, 15));
}