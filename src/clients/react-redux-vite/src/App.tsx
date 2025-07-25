import { BrowserRouter } from 'react-router-dom';
import { CssBaseline } from '@mui/material';
import { AppThemeProvider } from './theme/AppThemeProvider';
import NotificationSnackbar from './features/notifications/notificationSnackbar';
import AppRouter from './routes/AppRouter';

const App = () => {
  return (
    <AppThemeProvider>
      <BrowserRouter>
        <CssBaseline />
        <NotificationSnackbar />
        <AppRouter />
      </BrowserRouter>
    </AppThemeProvider>
  );
};

export default App;