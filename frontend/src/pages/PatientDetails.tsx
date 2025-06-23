import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  Box,
  Heading,
  Text,
  Spinner,
  Button,
  useColorModeValue,
  useToast,
} from '@chakra-ui/react';

import { Patient } from '../types/patient';
import { getSexLabel } from '../utils/sexUtils';

const PatientDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [patient, setPatient] = useState<Patient | null>(null);
  const [loading, setLoading] = useState(true);
  const toast = useToast();
  const navigate = useNavigate();
  const borderColor = useColorModeValue('gray.200', 'gray.700');

  useEffect(() => {
    if (!id) return;

    const fetchPatient = async () => {
      try {
        const response = await fetch(`/api/Patient/${id}`);
        if (!response.ok) {
          throw new Error(`Failed to fetch patient with id ${id}`);
        }
        const data: Patient = await response.json();
        setPatient(data);
      } catch (error) {
        toast({
          title: 'Error fetching patient details.',
          description: (error as Error).message,
          status: 'error',
          duration: 4000,
          isClosable: true,
        });
      } finally {
        setLoading(false);
      }
    };

    fetchPatient();
  }, [id, toast]);

  if (loading) {
    return (
      <Box textAlign="center" mt="8">
        <Spinner size="xl" />
      </Box>
    );
  }

  if (!patient) {
    return (
      <Box textAlign="center" mt="8" color={borderColor}>
        <Text>Patient not found.</Text>
        <Button mt="4" onClick={() => navigate(-1)}>
          Back
        </Button>
      </Box>
    );
  }

  return (
    <Box maxW="600px" mx="auto" mt="8" fontFamily="Arial, sans-serif" p="6" borderWidth="1px" borderRadius="md" borderColor={borderColor} boxShadow="sm">
      <Heading as="h2" size="lg" mb="6" textAlign="center">
        Patient Details
      </Heading>

      <Text><strong>Name:</strong> {patient.name}</Text>
      <Text><strong>Surname:</strong> {patient.surname}</Text>
      <Text><strong>Personal ID:</strong> {patient.personalId}</Text>
      <Text><strong>Sex:</strong> {getSexLabel(patient.sex)}</Text>
      <Text><strong>Date of Birth:</strong> {new Date(patient.dateOfBirth).toLocaleDateString()}</Text>

      <Button mt="6" onClick={() => navigate(-1)}>
        Back to List
      </Button>
    </Box>
  );
};

export default PatientDetails;
