import axios from 'axios';
import { Patient } from '../types/patient';

const API_BASE = 'http://localhost:8080/api';

export const fetchPatients = async (): Promise<Patient[]> => {
  const response = await axios.get<Patient[]>(`${API_BASE}/patient`);
  return response.data;
};

export const addPatient = async (patient: Omit<Patient, 'id'>): Promise<void> => {
  await axios.post(`${API_BASE}/patient`, patient);
};
