import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import PatientsPage from './pages/PatientsPage';
import PatientDetailsPage from './pages/PatientDetails';

const App: React.FC = () => (
  <ChakraProvider>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<PatientsPage />} />
        <Route path="/patients/:id" element={<PatientDetailsPage />} />
      </Routes>
    </BrowserRouter>
  </ChakraProvider>
);

export default App;
