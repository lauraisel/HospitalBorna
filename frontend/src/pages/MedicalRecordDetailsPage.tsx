import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getMedicalRecordById, fetchPatientById } from '../api/patientApi';
import { MedicalRecord } from '../types/MedicalRecord';
import { Patient } from '../types/patient';
import {
  Box,
  Heading,
  Text,
  Spinner,
  Alert,
  AlertIcon,
  Stack,
  Divider,
  Button,
  Card,
  CardBody,
} from '@chakra-ui/react';
import { getSexLabel } from '../utils/sexUtils';

const MedicalRecordDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [record, setRecord] = useState<MedicalRecord | null>(null);
  const [patient, setPatient] = useState<Patient | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const recordData = await getMedicalRecordById(Number(id));
        setRecord(recordData);

        const patientData = await fetchPatientById(recordData.patientId);
        setPatient(patientData);
      } catch (err) {
        setError('Failed to load data.');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  if (loading) return <Spinner size="xl" />;

  if (error) {
    return (
      <Alert status="error">
        <AlertIcon />
        {error}
      </Alert>
    );
  }

  if (!record || !patient) return null;

  return (
    <Box p={6}>
      <Card>
        <CardBody>
          <Button mb={4} onClick={() => navigate(-1)} colorScheme="gray">
            ← Back
          </Button>

          <Heading mb={4}>Medical Record Details</Heading>
          <Stack spacing={3}>
            <Text><strong>Disease Name:</strong> {record.diseaseName}</Text>
            <Text><strong>Start Date:</strong> {new Date(record.startDate).toLocaleDateString()}</Text>
            <Text><strong>End Date:</strong> {record.endDate ? new Date(record.endDate).toLocaleDateString() : 'Ongoing'}</Text>
          </Stack>

          <Divider my={6} />

          <Heading mb={4} size="md">Patient Information</Heading>
          <Stack spacing={3}>
            <Text><strong>Personal Id:</strong> {patient.personalId}</Text>
            <Text><strong>Full Name:</strong> {patient.name} {patient.surname}</Text>
            <Text><strong>Birth Date:</strong> {new Date(patient.dateOfBirth).toLocaleDateString()}</Text>
            <Text><strong>Sex:</strong> {getSexLabel(patient.sex)}</Text>
          </Stack>
        </CardBody>
      </Card>
    </Box>
  );
};

export default MedicalRecordDetailsPage;
