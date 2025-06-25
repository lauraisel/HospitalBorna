import axios from 'axios';
import { Patient } from '../types/patient';
import { MedicalRecord } from '../types/MedicalRecord';
import { Checkup } from '../types/Checkup';
import { Prescription } from '../types/Prescription';
import { CheckupImage } from '../types/CheckupImage';

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

  export const downloadMedicalRecordCsv = async (patientId: number): Promise<void> => {
    try {
      const response = await axios.get(`${API_BASE}/MedicalRecord/patient/${patientId}/csv`, {
        responseType: 'blob', 
      });
  
      const blob = new Blob([response.data], { type: 'text/csv' });
      const url = window.URL.createObjectURL(blob);
  
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `medical_records_patient_${patientId}.csv`);
      document.body.appendChild(link);
      link.click();
      link.remove();
      window.URL.revokeObjectURL(url);
    } catch (error) {
      console.error('Failed to download CSV:', error);
      throw error;
    }
  };

  export const fetchCheckupRecord = async (patientId: number): Promise<Checkup[]> => {
    const response = await axios.get<Checkup[]>(`${API_BASE}/Checkup/patient/${patientId}`);
    return response.data;
  };

  export const createCheckup = async (record: Omit<Checkup, 'id'>): Promise<Checkup> => {
    const response = await axios.post<Checkup>(`${API_BASE}/Checkup`, record);
    return response.data;
  };

  export const getMedicalRecordById = async (id: number): Promise<MedicalRecord> => {
    const response = await axios.get(`/api/MedicalRecord/${id}`);
    return response.data;
  };

  export const getCheckupRecordById = async (id: number): Promise<Checkup> => {
    const response = await axios.get(`/api/Checkup/${id}`);
    return response.data;
  };

  export const getPrescriptionsByCheckupId = async (checkupId: number): Promise<Prescription[]> => {
    const response = await axios.get<Prescription[]>(`${API_BASE}/Prescription/checkup/${checkupId}`);
    return response.data;
  };

  export const addPrescription = async (prescription: Omit<Prescription, 'id'>): Promise<Prescription> => {
    const response = await axios.post<Prescription>(`${API_BASE}/Prescription`, prescription);
    return response.data;
  };

  export const getCheckupImages = async (checkupId: number): Promise<CheckupImage[]> => {
    const response = await axios.get<CheckupImage[]>(`${API_BASE}/CheckupImage/checkup/${checkupId}`);
    return response.data;
  };

  export async function uploadCheckupImage(checkupId: number, file: File): Promise<void> {
    const formData = new FormData();
    formData.append('File', file);
    formData.append('CheckupId', checkupId.toString());
  
    const response = await fetch('/api/CheckupImage', {
      method: 'POST',
      body: formData,
    });
  
    if (!response.ok) {
      throw new Error(`Upload failed: ${response.statusText}`);
    }
  }