import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Button,
  FormControl,
  FormLabel,
  Select,
  useToast,
  Input,
} from '@chakra-ui/react';
import { addPatient } from '../api/patientApi';
import { sexOptions } from '../utils/sexUtils';
import { Sex } from '../types/Sex'

interface Props {
  isOpen: boolean;
  onClose: () => void;
  onPatientAdded: () => void; // callback to refresh list
}

const AddPatientModal: React.FC<Props> = ({ isOpen, onClose, onPatientAdded }) => {
  const [name, setName] = useState('');
  const [dateOfBirth, setBirthDate] = useState('');
  const [sex, setSex] = useState<Sex | ''>('');
  const [personalId, setPersonalId] = useState('');
const [surname, setSurname] = useState('');

  const [loading, setLoading] = useState(false);
  const toast = useToast();

   useEffect(() => {
      if (isOpen) {
        setName('');
        setBirthDate('');
        setSex('');
        setPersonalId('');
        setSurname('')
      }
    }, [isOpen]);
  

  const handleSubmit = async () => {
    if (!name || !dateOfBirth || sex === '') {
      toast({
        title: 'Please fill in all fields.',
        status: 'warning',
        duration: 3000,
        isClosable: true,
      });
      return;
    }
    setLoading(true);
    try {
        const isoDateOfBirth = new Date(dateOfBirth).toISOString();

      await addPatient({ name, surname, dateOfBirth: isoDateOfBirth, personalId, sex }); // send enum value
      toast({
        title: 'Patient added successfully.',
        status: 'success',
        duration: 3000,
        isClosable: true,
      });
      onPatientAdded();
      onClose();
      setName('');
      setBirthDate('');
      setSex('');
    } catch (error) {
      toast({
        title: 'Failed to add patient.',
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Add New Patient</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb="4" isRequired>
            <FormLabel>Name</FormLabel>
            <Input
              placeholder="Enter patient name"
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </FormControl>

          <FormControl mb="4">
            <FormLabel>Surname</FormLabel>
              <Input
                placeholder="Enter surname"
                value={surname}
                onChange={(e) => setSurname(e.target.value)}
                />
            </FormControl>

          <FormControl mb="4" isRequired>
            <FormLabel>Birth Date</FormLabel>
            <Input
              type="date"
              value={dateOfBirth}
              onChange={(e) => setBirthDate(e.target.value)}
            />
          </FormControl>
          
          <FormControl>
            <FormLabel>Personal Id</FormLabel>
            <Input
                placeholder="Enter personal ID"
                value={personalId}
                onChange={(e) => setPersonalId(e.target.value)}
            />
          </FormControl>

          <FormControl mb="4" isRequired>
            <FormLabel>Sex</FormLabel>
            <Select
              placeholder="Select sex"
              value={sex}
              onChange={(e) => setSex(Number(e.target.value) as Sex)}
            >
              {sexOptions.map(({ value, label }) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
            </Select>
          </FormControl>
        </ModalBody>

        <ModalFooter>
          <Button colorScheme="blue" mr={3} onClick={handleSubmit} isLoading={loading}>
            Add
          </Button>
          <Button variant="ghost" onClick={onClose} isDisabled={loading}>
            Cancel
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default AddPatientModal;
