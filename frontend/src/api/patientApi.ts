import axios from 'axios';
import { Patient } from '../types/patient';
import { MedicalRecord } from '../types/MedicalRecord';
import { Checkup } from '../types/Checkup';

const API_BASE = 'http://localhost:8080/api';

export const fetchPatients = async (): Promise<Patient[]> => {
  const response = await axios.get<Patient[]>(`${API_BASE}/patient`);
  return response.data;
};

export const fetchPatientById = async (id: number): Promise<Patient> => {
    const response = await axios.get<Patient>(`${API_BASE}/Patient/${id}`);
    return response.data;
  };
  

export const addPatient = async (patient: Omit<Patient, 'id'>): Promise<void> => {
  await axios.post(`${API_BASE}/patient`, patient);
};

export const fetchMedicalRecord = async (patientId: number): Promise<MedicalRecord[]> => {
    const response = await axios.get<MedicalRecord[]>(`${API_BASE}/MedicalRecord/patient/${patientId}`);
    return response.data;
  };

  export const createMedicalRecord = async (record: Omit<MedicalRecord, 'id'>): Promise<MedicalRecord> => {
    const response = await axios.post<MedicalRecord>(`${API_BASE}/MedicalRecord`, record);
    return response.data;
  };

  export const fetchCheckupRecord = async (patientId: number): Promise<Checkup[]> => {
    const response = await axios.get<Checkup[]>(`${API_BASE}/Checkup/patient/${patientId}`);
    return response.data;
  };