import React, { useEffect, useState } from 'react';
import {
  Box,
  Heading,
  Text,
  Stack,
  Divider,
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  Spinner,
  useToast,
  Button,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import { getCheckupRecordById, getPrescriptionsByCheckupId } from '../api/patientApi';
import { Checkup } from '../types/Checkup';
import { Prescription } from '../types/Prescription';
import { getCheckupProcedureLabel } from '../utils/checkupProcedureUtils';
import AddPrescriptionModal from '../components/AddPrescriptionModal';
import { getCheckupImages } from '../api/patientApi';
import { CheckupImage } from '../types/CheckupImage';
import { Image, SimpleGrid } from '@chakra-ui/react';


const CheckupDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [checkup, setCheckup] = useState<Checkup | null>(null);
  const [prescriptions, setPrescriptions] = useState<Prescription[]>([]);
  const [loading, setLoading] = useState(true);
  const toast = useToast();
  const navigate = useNavigate();
  const [images, setImages] = useState<CheckupImage[]>([]);


  const [isModalOpen, setIsModalOpen] = useState(false);

  const handlePrescriptionAdded = (newPrescription: Prescription) => {
    setPrescriptions(prev => [...prev, newPrescription]);
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        if (!id) return;
        const checkupData = await getCheckupRecordById(Number(id));
        const prescriptionData = await getPrescriptionsByCheckupId(Number(id));
        const imageData = await getCheckupImages(Number(id));
setImages(imageData);

        setCheckup(checkupData);
        setPrescriptions(prescriptionData);
      } catch (error) {
        toast({
          title: 'Failed to load checkup details',
          description: (error as Error).message,
          status: 'error',
          duration: 3000,
          isClosable: true,
        });
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id, toast]);

  if (loading) {
    return (
      <Box p={6} textAlign="center">
        <Spinner size="xl" />
      </Box>
    );
  }

  if (!checkup) {
    return (
      <Box p={6}>
        <Text>Checkup not found.</Text>
      </Box>
    );
  }

  return (
    <Box p={6}>
      <Button mb={4} onClick={() => navigate(-1)}>
        ← Back
      </Button>

      <Box p={6} borderWidth={1} borderRadius="lg" boxShadow="md">
        <Heading mb={4}>Checkup Details</Heading>
        <Stack spacing={3}>
          <Text><strong>Checkup Time:</strong> {new Date(checkup.checkupTime).toLocaleString()}</Text>
          <Text><strong>Procedure:</strong> {getCheckupProcedureLabel(checkup.procedure)}</Text>
        </Stack>

        <Divider my={6} />

        <Heading mb={4} size="md">Prescriptions</Heading>

        <Button colorScheme="green" size="sm" mb={4} onClick={() => setIsModalOpen(true)}>
          + Add Prescription
        </Button>

        {prescriptions.length === 0 ? (
          <Text>No prescriptions found for this checkup.</Text>
        ) : (
          <Accordion allowToggle>
            {prescriptions.map((prescription) => (
              <AccordionItem key={prescription.id}>
                <h2>
                  <AccordionButton>
                    <Box flex="1" textAlign="left">
                      Prescription #{prescription.id}
                    </Box>
                    <AccordionIcon />
                  </AccordionButton>
                </h2>
                <AccordionPanel pb={4}>
                  <Text><strong>Medicine Name:</strong> {prescription.medication}</Text>
                  <Text><strong>Dosage:</strong> {prescription.dosage}</Text>
                </AccordionPanel>
              </AccordionItem>
            ))}
          </Accordion>
        )}
      </Box>

      <AddPrescriptionModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        checkupId={checkup.id}
        onPrescriptionAdded={handlePrescriptionAdded}
      />

<Divider my={6} />
<Heading mb={4} size="md">Images</Heading>

{images.length === 0 ? (
  <Text>No images available for this checkup.</Text>
) : (
  <SimpleGrid columns={[1, 2, 3]} spacing={4}>
    {images.map((img) => (
      <Box key={img.id} borderWidth={1} borderRadius="md" p={2}>
        <Image src={img.imageUrl} alt={img.fileName} objectFit="cover" maxH="300px" w="100%" />
      </Box>
    ))}
  </SimpleGrid>
)}

    </Box>
  );
};

export default CheckupDetailsPage;
