import React from 'react';
import { useMsal } from '@azure/msal-react';
import { useNavigate } from 'react-router-dom';
import { loginRequest } from '../../msalConfig';
import { Container, Typography, Button, Box } from '@mui/material';
import { useAppDispatch } from '../../app/hooks';
import { setAuthError, setCredentials } from '../../app/authSlice';
import { showNotification } from '../notifications/notificationSlice';
import { User } from '../../types/user';
import MicrosoftIcon from '@mui/icons-material/Microsoft';

interface MSALTokenClaims {
  preferred_username?: string;
  name?: string;
  email?: string;
  oid?: string;
  tid?: string;
  roles?: string[];
}

const LoginPage: React.FC = () => {
  const { instance } = useMsal();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const handleLogin = () => {
    instance.loginPopup(loginRequest)
      .then((resp) => {
        const claims = resp.idTokenClaims as MSALTokenClaims;
        const user: User = {
          id: claims.oid || resp.account?.localAccountId || '',
          email: claims.email || resp.account?.username || '',
          name: claims.name || resp.account?.name || '',
          username: claims.preferred_username || resp.account?.username || '',
          roles: claims.roles || [ 'User' ],
          tenantId: claims.tid,
        };

        // This will automatically store in localStorage via the auth slice
        dispatch(setCredentials({ 
          accessToken: resp.accessToken, 
          user: user 
        }));

        dispatch(showNotification({ 
          message: 'Login successful', 
          severity: 'success' 
        }));

        navigate('/home');
      })
      .catch((error) => {
        console.error(error);
        dispatch(setAuthError('Login failed'));
        dispatch(showNotification({ message: 'Login failed', severity: 'error' }));
      });
  };

  return (
    <Container maxWidth="sm">
      <Box
        display="flex"
        flexDirection="column"
        alignItems="center"
        justifyContent="center"
        height="100vh"
        gap={3}
      >
        <Typography variant="h4" component="h1" gutterBottom>
          ~ Project: Elkhorn ~
        </Typography>
        <Typography variant="body1">
          Please sign in to continue.
        </Typography>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<MicrosoftIcon />}
          onClick={handleLogin}
        >
          Sign In with Microsoft
        </Button>
      </Box>
    </Container>
  );
};

export default LoginPage;