import { BrowserRouter } from 'react-router-dom';
import { CssBaseline } from '@mui/material';
import { AppThemeProvider } from './theme/AppThemeProvider';
import NotificationSnackbar from './features/notifications/notificationSnackbar';
import AppRouter from './routes/AppRouter';
import { useAuthInit } from './features/auth/authInit';

const App = () => {
  useAuthInit();
  
  return (
    <AppThemeProvider>
      <CssBaseline />
      <NotificationSnackbar />
      <BrowserRouter>
        <AppRouter />
      </BrowserRouter>
    </AppThemeProvider>
  );
};

export default App;