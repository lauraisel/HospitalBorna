export interface MedicalRecord {
    id: number;
    diseaseName: string;
    startDate: string;  
    endDate: string | null;    
    patientId: number;
  }
  