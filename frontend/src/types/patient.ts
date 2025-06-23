import { Sex } from './Sex';

export interface Patient {
  id: number;
  personalId: string;
  name: string;
  surname: string;
  dateOfBirth: string; // ISO date string
  sex: Sex;
}
