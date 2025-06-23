import React, { useState } from 'react';
import PatientList from '../components/PatientList';
import AddPatientModal from '../components/AddPatientModal';

const PatientsPage: React.FC = () => {
  const [refresh, setRefresh] = useState(false);
  const [isModalOpen, setModalOpen] = useState(false);

  const handleAddPatientClick = () => setModalOpen(true);
  const handleModalClose = () => setModalOpen(false);

  // After adding a patient, trigger refresh
  const handlePatientAdded = () => {
    setRefresh((prev) => !prev);
  };

  return (
    <>
      <PatientList refresh={refresh} onAddPatient={handleAddPatientClick} />
      <AddPatientModal
        isOpen={isModalOpen}
        onClose={handleModalClose}
        onPatientAdded={handlePatientAdded}
      />
    </>
  );
};

export default PatientsPage;
