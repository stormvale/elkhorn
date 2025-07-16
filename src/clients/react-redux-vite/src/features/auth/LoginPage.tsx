// src/pages/LoginPage.tsx
import React from 'react';
import { Button, Container, Typography, Box } from '@mui/material';
import { useMsal, useIsAuthenticated } from '@azure/msal-react';
import { setCredentials } from '../../app/authSlice';
import { useAppDispatch } from '../../app/hooks';
import { showNotification } from '../notifications/notificationSlice';
import { useNavigate } from 'react-router-dom';
import { loginRequest } from '../../msalConfig';

const LoginPage: React.FC = () => {
  const dispatch = useAppDispatch()
  const navigate = useNavigate();
  const { instance } = useMsal();

  const isAuthenticated = useIsAuthenticated();
  if (isAuthenticated) {
    navigate('/home');
    return null; // prevent further rendering
  }
  
  const handleSubmit = (event: React.FormEvent) => {
    event.preventDefault();
    
    instance.loginPopup(loginRequest)
      .then((tokenResponse) => {
        dispatch(setCredentials({
          accessToken: tokenResponse.accessToken,
          user: tokenResponse.account,
        }));
        dispatch(showNotification({ message: 'Login successful', severity: 'success' } ));
        navigate('/home');
      }).catch((error) => console.error(error));
  };

  return (
    <Container maxWidth="xs">
      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center'}}>
        <Typography component="h1" variant="h5">
          Project: Elkhorn
        </Typography>
        <Box component="form" onSubmit={handleSubmit} >
          <Button type="submit" fullWidth variant="contained">
            Sign In
          </Button>
        </Box>
      </Box>
    </Container>
  );
};

export default LoginPage;