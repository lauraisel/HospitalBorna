import React, { useEffect, useState } from 'react';
import axios from 'axios';
import {
  Box,
  Button,
  Heading,
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  TableContainer,
  useColorModeValue,
} from '@chakra-ui/react';
import { Patient } from '../types/patient';
import { getSexLabel } from '../utils/sexUtils';

interface Props {
  refresh: boolean;
  onAddPatient: () => void;
}

const PatientList: React.FC<Props> = ({ refresh, onAddPatient }) => {
  const [patients, setPatients] = useState<Patient[]>([]);

  const fetchPatients = async () => {
    try {
      const response = await axios.get<Patient[]>('http://localhost:8080/api/patient');
      setPatients(response.data);
    } catch (error) {
      console.error('Failed to fetch patients', error);
    }
  };

  useEffect(() => {
    fetchPatients();
  }, [refresh]);

  const headerBg = useColorModeValue('gray.100', 'gray.700');
  const borderColor = useColorModeValue('gray.200', 'gray.600');

  return (
    <Box maxW="700px" mx="auto" mt="8" fontFamily="Arial, sans-serif">
      <Heading as="h2" size="lg" textAlign="center" mb="6" color={useColorModeValue('gray.800', 'white')}>
        Patient List
      </Heading>

      <Button
        colorScheme="blue"
        mb="4"
        float="right"
        onClick={onAddPatient}
      >
        Add Patient
      </Button>

      <TableContainer boxShadow="sm" borderRadius="md" borderWidth="1px" borderColor={borderColor}>
        <Table variant="simple" size="md">
          <Thead bg={headerBg}>
            <Tr>
              <Th>Name</Th>
              <Th>Surname</Th>
              <Th>Personal ID</Th>
              <Th>Sex</Th>
              <Th>Date of Birth</Th>
            </Tr>
          </Thead>
          <Tbody>
            {patients.map((p) => (
              <Tr key={p.id} borderBottom="1px" borderColor={borderColor}>
                <Td>{p.name}</Td>
                <Td>{p.surname}</Td>
                <Td>{p.personalId}</Td>
                <Td>{getSexLabel(p.sex)}</Td>
                <Td>{new Date(p.dateOfBirth).toLocaleDateString()}</Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default PatientList;
