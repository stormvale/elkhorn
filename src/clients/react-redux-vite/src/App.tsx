import { BrowserRouter } from 'react-router-dom';
import { Box, CssBaseline } from '@mui/material';
import { AppThemeProvider } from './features/theme/AppThemeProvider';
import Routes from './routes';
import Sidebar from './components/Sidebar';
import Header from './components/Header';
import NotificationSnackbar from './features/notifications/notificationSnackbar';

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
      <NotificationSnackbar />
    </AppThemeProvider>
  );
};

export default App;