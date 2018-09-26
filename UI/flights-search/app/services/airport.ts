import fetch, { handleRejection } from "flights-search/utils/fetch";

export default class Airport {
  getAirports() {
    return fetch(`/airports`).catch(handleRejection);
  }
}

