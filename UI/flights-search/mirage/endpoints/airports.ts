import { Server } from 'ember-cli-mirage';

export default function routesForAirports(server: Server) {
  server.get('/api/airports', ({ db }, request) => db.airports);
}