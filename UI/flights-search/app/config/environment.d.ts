export default config;

/**
 * Type declarations for
 *    import config from './config/environment'
 *
 * For now these need to be managed by the developer
 * since different ember addons can materialize new entries.
 */
declare const config: {
  environment: any;
  cssVariables: any;
  modulePrefix: string;
  podModulePrefix: string;
  locationType: string;
  rootURL: string;
  apiURL: string;
};
