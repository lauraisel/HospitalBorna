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
  useToast,
} from '@chakra-ui/react';
import { createMedicalRecord } from '../api/patientApi';
import { MedicalRecord } from '../types/MedicalRecord';

interface AddMedicalRecordModalProps {
  isOpen: boolean;
  onClose: () => void;
  patientId: number;
  onAdded: () => void;
}

const AddMedicalRecordModal: React.FC<AddMedicalRecordModalProps> = ({
  isOpen,
  onClose,
  patientId,
  onAdded,
}) => {
  const [diseaseName, setDiseaseName] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const toast = useToast();

  useEffect(() => {
    if (isOpen) {
      setDiseaseName('');
      setStartDate('');
      setEndDate(null);
    }
  }, [isOpen]);

  type NewMedicalRecord = Omit<MedicalRecord, 'id'>;

  const handleSubmit = async () => {
    if (!diseaseName || !startDate) {
      toast({
        title: 'Validation error',
        description: 'Disease name and start date are required.',
        status: 'warning',
        duration: 3000,
        isClosable: true,
      });
      return;
    }
    setLoading(true);
    const newRecord: NewMedicalRecord = {
      diseaseName,
      startDate: new Date(startDate).toISOString(),
      endDate: endDate ? new Date(endDate).toISOString() : null,
      patientId,
    };
    try {
      await createMedicalRecord(newRecord);
      toast({
        title: 'Medical record added',
        status: 'success',
        duration: 3000,
        isClosable: true,
      });
      onAdded();
      onClose();
    } catch (error) {
      toast({
        title: 'Failed to add medical record',
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
        <ModalHeader>Add Medical Record</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl mb={4} isRequired>
            <FormLabel>Disease Name</FormLabel>
            <Input
              value={diseaseName}
              onChange={(e) => setDiseaseName(e.target.value)}
              placeholder="Enter disease name"
            />
          </FormControl>

          <FormControl mb={4} isRequired>
            <FormLabel>Start Date</FormLabel>
            <Input
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
            />
          </FormControl>

          <FormControl mb={4}>
            <FormLabel>End Date</FormLabel>
            <Input
              type="date"
              value={endDate ?? ''}
              onChange={(e) => setEndDate(e.target.value ? e.target.value : null)}
            />
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

export default AddMedicalRecordModal;
