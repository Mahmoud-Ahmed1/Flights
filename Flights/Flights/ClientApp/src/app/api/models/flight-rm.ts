/* tslint:disable */
/* eslint-disable */
import { TimePlaceRm } from '../models/time-place-rm';
export interface FlightRm {
  airline?: null | string;
  arrival?: TimePlaceRm;
  departure?: TimePlaceRm;
  id?: string;
  price?: null | string;
  reaminingNumberOfSeats?: number;
}
