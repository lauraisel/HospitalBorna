import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import PatientsPage from './pages/PatientsPage';
import PatientDetailsPage from './pages/PatientDetails';
import MedicalRecordDetailsPage from './pages/MedicalRecordDetailsPage';
import CheckupDetailsPage from './pages/CheckupDetailsPage';

const App: React.FC = () => (
  <ChakraProvider>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<PatientsPage />} />
        <Route path="/patients/:id" element={<PatientDetailsPage />} />
        <Route path="/checkups/:id" element={<CheckupDetailsPage />} />
        <Route path="/medical-records/:id" element={<MedicalRecordDetailsPage />} />
      </Routes>
    </BrowserRouter>
  </ChakraProvider>
);

export default App;
