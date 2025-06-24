import React, { useState, useEffect } from 'react';
import {
  Button,
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  FormControl,
  FormLabel,
  Input,
  Select,
  useToast,
} from '@chakra-ui/react';
import { CheckupProcedure } from '../types/CheckupProcedure';
import { checkupProcedureOptions } from '../utils/checkupProcedureUtils'; // adjust path if needed
import { createCheckup } from '../api/patientApi';
import { Checkup } from '../types/Checkup';

interface AddCheckupModalProps {
  isOpen: boolean;
  onClose: () => void;
  patientId: number;
  onAdded: () => void;
}

const AddCheckupModal: React.FC<AddCheckupModalProps> = ({
  isOpen,
  onClose,
  patientId,
  onAdded,
}) => {
  const [checkupTime, setCheckupTime] = useState('');
  const [procedure, setProcedure] = useState<CheckupProcedure | ''>('');
  const [loading, setLoading] = useState(false);
  const toast = useToast();

  useEffect(() => {
    if (isOpen) {
      setCheckupTime('');
      setProcedure('');
    }
  }, [isOpen]);

  type NewCheckup = Omit<Checkup, 'id'>;

  const handleSubmit = async () => {
    if (!checkupTime || procedure === '') {
      toast({
        title: 'Validation error',
        description: 'Date and procedure are required.',
        status: 'warning',
        duration: 3000,
        isClosable: true,
      });
      return;
    }

    setLoading(true);
    const newCheckup: NewCheckup = {
      checkupTime: new Date(checkupTime).toISOString(),
      procedure: procedure as CheckupProcedure,
      patientId,
    };

    try {
      await createCheckup(newCheckup);
      toast({
        title: 'Checkup added',
        status: 'success',
        duration: 3000,
        isClosable: true,
      });
      onAdded();
      onClose();
    } catch (error) {
      toast({
        title: 'Failed to add checkup',
        description: (error as Error).message,
        status: 'error',
        duration: 4000,
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
        <ModalHeader>Add Checkup</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={4} isRequired>
            <FormLabel>Checkup Time</FormLabel>
            <Input
              type="datetime-local"
              value={checkupTime}
              onChange={(e) => setCheckupTime(e.target.value)}
            />
          </FormControl>

          <FormControl mb={4} isRequired>
            <FormLabel>Procedure</FormLabel>
            <Select
              placeholder="Select procedure"
              value={procedure}
              onChange={(e) => setProcedure(Number(e.target.value))}
            >
              {checkupProcedureOptions.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </Select>
          </FormControl>
        </ModalBody>

        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="blue" onClick={handleSubmit} isLoading={loading}>
            Add
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default AddCheckupModal;
