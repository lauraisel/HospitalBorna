import { CheckupProcedure } from '../types/CheckupProcedure';

export const checkupProcedureOptions = [
  { value: CheckupProcedure.GP, label: 'General Practitioner' },
  { value: CheckupProcedure.BLOOD, label: 'Blood Test' },
  { value: CheckupProcedure.X_RAY, label: 'X-Ray' },
  { value: CheckupProcedure.CT, label: 'CT Scan' },
  { value: CheckupProcedure.MR, label: 'MRI' },
  { value: CheckupProcedure.ULTRA, label: 'Ultrasound' },
  { value: CheckupProcedure.ECG, label: 'ECG' },
  { value: CheckupProcedure.ECHO, label: 'Echocardiogram' },
  { value: CheckupProcedure.EYE, label: 'Eye Exam' },
  { value: CheckupProcedure.DERM, label: 'Dermatology' },
  { value: CheckupProcedure.DENTA, label: 'Dental Exam' },
  { value: CheckupProcedure.MAMMO, label: 'Mammogram' },
  { value: CheckupProcedure.NEURO, label: 'Neurology' },
];

export function getCheckupProcedureLabel(value: CheckupProcedure): string {
  const option = checkupProcedureOptions.find((opt) => opt.value === value);
  return option ? option.label : 'Unknown';
}