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
import { useNavigate } from 'react-router-dom';

interface Props {
  refresh: boolean;
  onAddPatient: () => void;
}

const PatientList: React.FC<Props> = ({ refresh, onAddPatient }) => {
  const [patients, setPatients] = useState<Patient[]>([]);
  const navigate = useNavigate();

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

  const handleDetailsClick = (id: number) => {
    navigate(`/patients/${id}`);
  };

  return (
    <Box maxW="700px" mx="auto" mt="8" fontFamily="Arial, sans-serif">
      {/* Flex container for heading and button */}
      <Box display="flex" justifyContent="space-between" alignItems="center" mb="6">
        <Heading
          as="h2"
          size="lg"
          textAlign="center"
          flex="1"
          color={useColorModeValue('gray.800', 'white')}
        >
          Patient List
        </Heading>

        <Button colorScheme="blue" onClick={onAddPatient} ml="auto">
          Add Patient
        </Button>
      </Box>

      <TableContainer boxShadow="sm" borderRadius="md" borderWidth="1px" borderColor={borderColor}>
        <Table variant="simple" size="md">
          <Thead bg={headerBg}>
            <Tr>
              <Th>Name</Th>
              <Th>Surname</Th>
              <Th>Personal ID</Th>
              <Th>Sex</Th>
              <Th>Date of Birth</Th>
              <Th>Details</Th> {/* New column */}
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
                <Td>
                  <Button
                    size="sm"
                    colorScheme="teal"
                    onClick={() => handleDetailsClick(p.id)}
                  >
                    Details
                  </Button>
                </Td>
              </Tr>
            ))}
          </Tbody>
        </Table>
      </TableContainer>
    </Box>
  );
};

export default PatientList;
