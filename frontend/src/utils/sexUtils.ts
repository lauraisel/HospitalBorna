import { Sex } from '../types/Sex';

export const sexOptions = [
  { value: Sex.Male, label: 'Male' },
  { value: Sex.Female, label: 'Female' },
];

// Optionally, a helper to get label by enum value
export function getSexLabel(value: Sex): string {
  const option = sexOptions.find((opt) => opt.value === value);
  return option ? option.label : 'Unknown';
}
