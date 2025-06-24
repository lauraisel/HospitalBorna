import React, { useEffect, useState, useRef } from 'react';
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
  SimpleGrid,
  Image,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import {
  getCheckupRecordById,
  getPrescriptionsByCheckupId,
  getCheckupImages,
  uploadCheckupImage,
} from '../api/patientApi';
import { Checkup } from '../types/Checkup';
import { Prescription } from '../types/Prescription';
import { CheckupImage } from '../types/CheckupImage';
import { getCheckupProcedureLabel } from '../utils/checkupProcedureUtils';
import AddPrescriptionModal from '../components/AddPrescriptionModal';

const CheckupDetailsPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [checkup, setCheckup] = useState<Checkup | null>(null);
  const [prescriptions, setPrescriptions] = useState<Prescription[]>([]);
  const [loading, setLoading] = useState(true);
  const [uploading, setUploading] = useState(false);
  const [images, setImages] = useState<CheckupImage[]>([]);
  const toast = useToast();
  const navigate = useNavigate();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const fileInputRef = useRef<HTMLInputElement>(null);

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

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files || e.target.files.length === 0) return;
    const file = e.target.files[0];
    if (!checkup) return;

    setUploading(true);
    try {
      await uploadCheckupImage(checkup.id, file);

      // Refetch images after successful upload
      const updatedImages = await getCheckupImages(checkup.id);
      setImages(updatedImages);
      toast({
        title: 'Image uploaded successfully',
        status: 'success',
        duration: 3000,
        isClosable: true,
      });
    } catch (error) {
      toast({
        title: 'Upload failed',
        description: (error as Error).message,
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setUploading(false);
      if (fileInputRef.current) fileInputRef.current.value = '';
    }
  };

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
          <Text>
            <strong>Checkup Time:</strong> {new Date(checkup.checkupTime).toLocaleString()}
          </Text>
          <Text>
            <strong>Procedure:</strong> {getCheckupProcedureLabel(checkup.procedure)}
          </Text>
        </Stack>

        <Divider my={6} />

        <Heading mb={4} size="md">
          Prescriptions
        </Heading>

        <Button colorScheme="green" size="sm" mb={4} onClick={() => setIsModalOpen(true)}>
          + Add Prescription
        </Button>

        {prescriptions.length === 0 ? (
          <Text>No prescriptions found for this checkup.</Text>
        ) : (
          <Accordion allowToggle>
            {prescriptions.map(prescription => (
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
                  <Text>
                    <strong>Medicine Name:</strong> {prescription.medication}
                  </Text>
                  <Text>
                    <strong>Dosage:</strong> {prescription.dosage}
                  </Text>
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
      <Heading mb={4} size="md">
        Images
      </Heading>

      <Button
        mb={4}
        colorScheme="blue"
        size="sm"
        onClick={() => fileInputRef.current?.click()}
        isLoading={uploading}
        loadingText="Uploading..."
      >
        + Add Image
      </Button>
      <input
        type="file"
        accept="image/*"
        onChange={handleFileChange}
        style={{ display: 'none' }}
        ref={fileInputRef}
      />

      {images.length === 0 ? (
        <Text>No images available for this checkup.</Text>
      ) : (
        <SimpleGrid columns={[1, 2, 3]} spacing={4}>
          {images.map(img => (
            <Box key={img.id} borderWidth={1} borderRadius="md" p={2}>
              <Image
                src={`data:image/png;base64,${img.fileContent}`} // use base64 content directly
                alt={img.fileName}
                objectFit="cover"
                maxH="300px"
                w="100%"
                fallbackSrc="https://via.placeholder.com/300x200?text=No+Image"
              />
            </Box>
          ))}
        </SimpleGrid>
      )}
    </Box>
  );
};

export default CheckupDetailsPage;
