import React from 'react';
import { ChakraProvider } from '@chakra-ui/react';
import PatientsPage from './pages/PatientsPage';

const App: React.FC = () => (
  <ChakraProvider >
    <PatientsPage />
  </ChakraProvider>
);

export default App;