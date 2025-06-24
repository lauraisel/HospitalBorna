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
  Collapse,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  TableContainer,
} from '@chakra-ui/react';
import { ChevronDownIcon, ChevronUpIcon } from '@chakra-ui/icons';

import { Patient } from '../types/patient';
import { MedicalRecord } from '../types/MedicalRecord';
import { getSexLabel } from '../utils/sexUtils';
import { fetchPatientById, fetchMedicalRecord, fetchCheckupRecord,  downloadMedicalRecordCsv } from '../api/patientApi';
import AddMedicalRecordModal from '../components/AddMedicalRecordModal';
import AddCheckupModal from '../components/AddCheckupModal';
import { Checkup } from '../types/Checkup';
import { getCheckupProcedureLabel } from '../utils/checkupProcedureUtils';

const PatientDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [patient, setPatient] = useState<Patient | null>(null);
  const [medicalRecords, setMedicalRecords] = useState<MedicalRecord[]>([]);
  const [checkups, setCheckups] = useState<Checkup[]>([]);
  const [loadingPatient, setLoadingPatient] = useState(true);
  const [loadingRecords, setLoadingRecords] = useState(false);
  const [recordsOpen, setRecordsOpen] = useState(false);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const [loadingCheckupRecords, setLoadingCheckupRecords] = useState(false);
  const [recordsCheckupOpen, setRecordsCheckupOpen] = useState(false);
  const [isCheckupModalOpen, setCheckupIsModalOpen] = useState(false);

  const handleCheckupDetailsClick = (id: number) => {
    navigate(`/checkups/${id}`);
  };

  

  const handleMedicalRecordDetailsClick = (recordId: number) => {
    navigate(`/medical-records/${recordId}`);
  };

  const toast = useToast();
  const navigate = useNavigate();
  const borderColor = useColorModeValue('gray.200', 'gray.700');
  const headerBg = useColorModeValue('gray.100', 'gray.700');

  useEffect(() => {
    if (!id) return;

    const loadPatient = async () => {
      setLoadingPatient(true);
      try {
        const data = await fetchPatientById(Number(id));
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
        setLoadingPatient(false);
      }
    };

    loadPatient();
  }, [id, toast]);

  const loadMedicalRecords = async () => {
    if (!id) return;
    setLoadingRecords(true);
    try {
      const records = await fetchMedicalRecord(Number(id));
      setMedicalRecords(records);
      setRecordsOpen(true);
    } catch (error) {
      toast({
        title: 'Error fetching medical records.',
        description: (error as Error).message,
        status: 'error',
        duration: 4000,
        isClosable: true,
      });
    } finally {
      setLoadingRecords(false);
    }
  };

  const loadCheckupRecords = async () => {
    if (!id) return;
    setLoadingCheckupRecords(true);
    try {
      const records = await fetchCheckupRecord(Number(id));
      setCheckups(records);
      setRecordsCheckupOpen(true);
    } catch (error) {
      toast({
        title: 'Error fetching checkup records.',
        description: (error as Error).message,
        status: 'error',
        duration: 4000,
        isClosable: true,
      });
    } finally {
      setLoadingCheckupRecords(false);
    }
  };

  const handleCheckupModalClose = () => {
    setCheckupIsModalOpen(false);
  };

  const handleCheckupRecordAdded = () => {
    loadCheckupRecords(); // reload records after adding
    setCheckupIsModalOpen(false); // close modal after add
  };

  // Move handlers out here so they can be used in JSX
  const handleModalClose = () => {
    setIsModalOpen(false);
  };

  const handleRecordAdded = () => {
    loadMedicalRecords(); // reload records after adding
    setIsModalOpen(false); // close modal after add
  };

  if (loadingPatient) {
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
    <Box
      maxW="600px"
      mx="auto"
      mt="8"
      fontFamily="Arial, sans-serif"
      p="6"
      borderWidth="1px"
      borderRadius="md"
      borderColor={borderColor}
      boxShadow="sm"
    >
      <Heading as="h2" size="lg" mb="6" textAlign="center">
        Patient Details
      </Heading>

      <Text>
        <strong>Name:</strong> {patient.name}
      </Text>
      <Text>
        <strong>Surname:</strong> {patient.surname}
      </Text>
      <Text>
        <strong>Personal ID:</strong> {patient.personalId}
      </Text>
      <Text>
        <strong>Sex:</strong> {getSexLabel(patient.sex)}
      </Text>
      <Text>
        <strong>Date of Birth:</strong> {new Date(patient.dateOfBirth).toLocaleDateString()}
      </Text>

        <Button size="sm" colorScheme="blue" onClick={() => downloadMedicalRecordCsv(patient.id)}>
          Download Medical Records (CSV)
        </Button>

      <Box mt="6" display="flex" gap="4" flexWrap="wrap">
        <Button
          size="sm"
          onClick={() => {
            if (recordsOpen) {
              setRecordsOpen(false);
            } else {
              loadMedicalRecords();
            }
          }}
          rightIcon={recordsOpen ? <ChevronUpIcon /> : <ChevronDownIcon />}
        >
          {recordsOpen ? 'Hide Medical Records' : 'Show Medical Records'}
        </Button>

        <Button size="sm" colorScheme="blue" onClick={() => setIsModalOpen(true)}>
          Add Medical Record
        </Button>
        

      </Box>

      <Collapse in={recordsOpen} animateOpacity>
        <TableContainer mt="4" boxShadow="sm" borderRadius="md" borderWidth="1px" borderColor={borderColor}>
          {loadingRecords ? (
            <Box textAlign="center" p="4">
              <Spinner />
            </Box>
          ) : medicalRecords.length === 0 ? (
            <Box textAlign="center" p="4" color={borderColor}>
              No medical records found.
            </Box>
          ) : (
            <Table variant="simple" size="md">
              <Thead bg={headerBg}>
                <Tr>
                  <Th>Disease Name</Th>
                  <Th>Start Date</Th>
                  <Th>End Date</Th>
                  <Th>Details</Th>
                </Tr>
              </Thead>
              <Tbody>
                {medicalRecords.map((record) => (
                  <Tr key={record.id} borderBottom="1px" borderColor={borderColor}>
                    <Td>{record.diseaseName}</Td>
                    <Td>{new Date(record.startDate).toLocaleDateString()}</Td>
                    <Td>{record.endDate ? new Date(record.endDate).toLocaleDateString() : 'Ongoing'}</Td>
                    <Td>
                      <Button size="sm" colorScheme="blue" onClick={() => handleMedicalRecordDetailsClick(record.id)}>
                        Details
                      </Button>
                    </Td>
                  </Tr>
                ))}
              </Tbody>
            </Table>
          )}
        </TableContainer>
      </Collapse>

      <Box mt="6" display="flex" gap="4" flexWrap="wrap">
        <Button
          size="sm"
          onClick={() => {
            if (recordsCheckupOpen) {
              setRecordsCheckupOpen(false);
            } else {
              loadCheckupRecords();
            }
          }}
          rightIcon={recordsCheckupOpen ? <ChevronUpIcon /> : <ChevronDownIcon />}
        >
          {recordsCheckupOpen ? 'Hide Checkups' : 'Show Checkups'}
        </Button>

        <Button size="sm" colorScheme="blue" onClick={() => setCheckupIsModalOpen(true)}>
          Add Checkup
        </Button>
      </Box>

      <Collapse in={recordsCheckupOpen} animateOpacity>
        <TableContainer mt="4" boxShadow="sm" borderRadius="md" borderWidth="1px" borderColor={borderColor}>
          {loadingCheckupRecords ? (
            <Box textAlign="center" p="4">
              <Spinner />
            </Box>
          ) : checkups.length === 0 ? (
            <Box textAlign="center" p="4" color={borderColor}>
              No checkups found.
            </Box>
          ) : (
            <Table variant="simple" size="md">
              <Thead bg={headerBg}>
                <Tr>
                  <Th>Checkup Date</Th>
                  <Th>Checkup Procedure</Th>
                  <Th>Details</Th>
                </Tr>
              </Thead>
              <Tbody>
                {checkups.map((checkup) => (
                  <Tr key={checkup.id} borderBottom="1px" borderColor={borderColor}>
                    <Td>{new Date(checkup.checkupTime).toLocaleDateString()}</Td>
                    <Td>{getCheckupProcedureLabel(checkup.procedure)}</Td>
                    <Td>
                    <Button size="sm" colorScheme="blue" onClick={() => handleCheckupDetailsClick(checkup.id)}>
                        Details
                      </Button>
                    </Td>
                  </Tr>
                ))}
              </Tbody>
            </Table>
          )}
        </TableContainer>
      </Collapse>

      <Button mt="6" onClick={() => navigate(-1)}>
        Back to List
      </Button>

      <AddMedicalRecordModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        patientId={patient.id}
        onAdded={handleRecordAdded}
      />

      <AddCheckupModal
        isOpen={isCheckupModalOpen}
        onClose={handleCheckupModalClose}
        patientId={patient.id}
        onAdded={handleCheckupRecordAdded}
      />
    </Box>
  );
};

export default PatientDetails;
