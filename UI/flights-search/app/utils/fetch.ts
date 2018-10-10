import _ from 'lodash';
import { fetch as f } from 'fetch';
import { Promise } from 'rsvp';
import config from 'flights-search/config/environment';

type FetchOptions = {
  body?: any;
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE';
  headers?: {};
};

export class FetchError extends Error {
  response: any;
}

export function handleRejection(
  rejection: any,
  message = 'Sorry, there was an error processing your request'
) {
  if (rejection.response) {
    return rejection.response
      .json()
      .then(r =>
        Promise.reject(
          new Error((r && r.data && r.data.errorMessage) || r.error_description || message)
        )
      );
  }

  return Promise.reject(new Error(message));
}

export function isUnauthorized(response) {
  return response.status === 401;
}

export const handleResponse = (response: any) => {
  if (response.ok) {
    return response.text().then(text => (text ? JSON.parse(text) : {}));
  } else {
    const error = new FetchError(response.statusText || response.status);
    error.response = response;
    throw error;
  }
};

export const formEncodedToJson = encoded => {
  const result = {};
  encoded.split('&').forEach(part => {
    const item = part.split('=');
    result[item[0]] = decodeURIComponent(item[1]);
  });
  return result;
};

function fetch(endpoint, options: FetchOptions = { headers: {} }) {
  const url = `${config.apiURL}${endpoint}`;

  return f(url, options).then(response => {
    return handleResponse(response);
  });
}

export default fetch;
