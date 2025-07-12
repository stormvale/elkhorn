import { BrowserRouter } from 'react-router-dom';
import { Box, CssBaseline } from '@mui/material';
import Routes from './routes';
import { Header } from './components/Header';
import { Sidebar } from './components/Sidebar';
import { AppThemeProvider } from './features/theme/AppThemeProvider';

const App = () => {
  return (
    <AppThemeProvider>
      <CssBaseline />
      <BrowserRouter>
        <Box sx={{ display: 'flex' }}>
          <Sidebar />
          <Box component="main" sx={{ flexGrow: 1, minHeight: '100vh', bgcolor: 'background.default' }}>
            <Header />
            <Routes />
          </Box>
        </Box>
      </BrowserRouter>
    </AppThemeProvider>
  );
};

export default App;