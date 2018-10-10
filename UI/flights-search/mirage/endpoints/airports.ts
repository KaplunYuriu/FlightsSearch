import { Server } from 'ember-cli-mirage';

export default function routesForAirports(server: Server) {
  server.get('/airports', ({ db }, request) => db.airports);
}