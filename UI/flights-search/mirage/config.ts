import { Server } from 'ember-cli-mirage';
import config from 'flights-search/config/environment';
import endpoints from 'flights-search/mirage/endpoints';

const configureRoutes = (server: Server) => {
  server.urlPrefix = config.apiURL;
  server.namespace = '';

  for (const namespace of Object.keys(endpoints)) {
    endpoints[namespace](server);
  }
};

export default function devConfig(this: Server) {
  configureRoutes(this);

  // Reset for everything else
  this.namespace = '';
  this.passthrough();
}