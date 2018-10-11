import { AsyncActionStatus } from "flights-search/types";
import _ from "lodash";

export type ApplicationGlobalState = {
  isLoading: boolean;
};

const initialState: ApplicationGlobalState = {
  isLoading: false
};

export default function (
  state: ApplicationGlobalState = initialState,
  action
): ApplicationGlobalState {
  if (action.status === AsyncActionStatus.Pending) {
    return Object.assign({}, state, {
      isLoading: true
    });
  } else {
    return Object.assign({}, state, {
      isLoading: false
    });
  }
}
