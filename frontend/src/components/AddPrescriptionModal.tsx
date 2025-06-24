import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  useToast,
} from '@chakra-ui/react';
import { Prescription } from '../types/Prescription';
import { addPrescription } from '../api/patientApi';

interface AddPrescriptionModalProps {
  isOpen: boolean;
  onClose: () => void;
  checkupId: number;
  onPrescriptionAdded: (prescription: Prescription) => void;
}

const AddPrescriptionModal: React.FC<AddPrescriptionModalProps> = ({
  isOpen,
  onClose,
  checkupId,
  onPrescriptionAdded,
}) => {
  const [medication, setMedication] = useState('');
  const [dosage, setDosage] = useState('');
  const toast = useToast();

  const resetForm = () => {
    setMedication('');
    setDosage('');
  };

  useEffect(() => {
    if (!isOpen) {
      resetForm(); // reset form when modal closes
    }
  }, [isOpen]);

  const handleSubmit = async () => {
    try {
      const newPrescription: Omit<Prescription, 'id'> = {
        medication,
        dosage,
        checkupId,
      };

      const added = await addPrescription(newPrescription);
      onPrescriptionAdded(added);

      toast({
        title: 'Prescription added.',
        status: 'success',
        duration: 3000,
        isClosable: true,
      });

      resetForm();
      onClose();
    } catch (error) {
      toast({
        title: 'Error adding prescription.',
        description: (error as Error).message,
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Add Prescription</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={3}>
            <FormLabel>Medication</FormLabel>
            <Input
              value={medication}
              onChange={(e) => setMedication(e.target.value)}
            />
          </FormControl>
          <FormControl mb={3}>
            <FormLabel>Dosage</FormLabel>
            <Input
              value={dosage}
              onChange={(e) => setDosage(e.target.value)}
            />
          </FormControl>
        </ModalBody>

        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="blue" onClick={handleSubmit}>
            Add
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default AddPrescriptionModal;
