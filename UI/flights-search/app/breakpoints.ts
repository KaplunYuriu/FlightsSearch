import config from 'flights-search/config/environment';
const cssVariables = config.cssVariables;

export default {
  small: `(max-width: ${cssVariables['$break-small']})`,
  medium: `(min-width: ${cssVariables['$break-medium']})`,
  large: `(min-width: ${cssVariables['$break-large']})`,
};
