import { Store } from 'redux';
import _ from 'lodash';
import { AsyncActionStatus } from 'flights-search/types';

const isPromise = val => val && _.isFunction(val.then) && _.isFunction(val.catch);

export default function({ dispatch }: Store<any>) {
  return next => action => {
    if (!isPromise(action.payload)) {
      return next(action);
    }

    const { payload, type, meta, successMessage } = action;
    const dispatchStage = (status: AsyncActionStatus, dispatchPayload: any) =>
      dispatch({
        status,
        type,
        meta,
        payload: dispatchPayload,
        error: status === AsyncActionStatus.Failed,
        successMessage: successMessage,
      });

    if (_.isUndefined(action.status)) {
      dispatchStage(AsyncActionStatus.Pending, {});
    }
    return payload
      .then(result => dispatchStage(AsyncActionStatus.Successful, result))
      .catch(err => dispatchStage(AsyncActionStatus.Failed, err));
  };
}
