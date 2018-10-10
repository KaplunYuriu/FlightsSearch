import { Store } from 'redux';

export default function array({ dispatch }: Store<any>) {
  return next => action => (Array.isArray(action) ? action.map(dispatch) : next(action));
}
