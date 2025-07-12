import { BrowserRouter } from 'react-router-dom';
import { CssBaseline, Toolbar } from '@mui/material';
import Routes from './routes';
import { Header } from './components/Header';
import { Sidebar } from './components/Sidebar';
import { useState } from 'react';
import { AppThemeProvider } from './features/theme/AppThemeProvider';

const App = () => {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  
  return (
    <AppThemeProvider>
      <CssBaseline />
      <BrowserRouter>
        <Header onMenuClick={() => setSidebarOpen(true)} />
        <Sidebar open={sidebarOpen} onClose={() => setSidebarOpen(false)} />
        <Toolbar /> {/* Spacer for fixed header */}
        <Routes />
      </BrowserRouter>
    </AppThemeProvider>
  );
};

export default App;