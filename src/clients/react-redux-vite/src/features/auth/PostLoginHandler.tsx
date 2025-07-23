import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Container, Typography, CircularProgress, Box } from '@mui/material';
import { useAppDispatch } from '../../app/hooks';
import { setSchoolContext } from '../../app/authSlice';
import { useProfileQuery } from '../users/api/apiSlice';

const PostLoginHandler = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const [status, setStatus] = useState('Checking your profile...');

  // This will auto-fetch when component mounts
  const { data: userProfile, isLoading, error } = useProfileQuery();

  // Handle the profile query results in a separate useEffect
  useEffect(() => {
    if (isLoading) {
      setStatus('Loading your profile...');
      return;
    }

    if (error) {
      const isNewUser = 'status' in error && error.status === 404;
      console.log('Profile error:', { isNewUser, error });

      if (isNewUser) {
        setStatus('Welcome! Let\'s get you set up...');
        setTimeout(() => navigate('/school-selector'), 1000);
      } else {
        setStatus('Failed to load profile');
        setTimeout(() => navigate('/'), 2000);
      }
      return;
    }

    if (userProfile) {
      if (userProfile.schools && userProfile.schools.length > 0) {
        setStatus('Setting up your school context...');
        dispatch(setSchoolContext({ schools: userProfile.schools }));
        navigate('/home');
      } else {
        setStatus('Welcome! Let\'s get you connected to a school...');
        setTimeout(() => navigate('/school-selector'), 1000);
      }
    }
  }, [userProfile, isLoading, error, navigate]);

  if (!isLoading && status.includes('wrong')) {
    return (
      <Container maxWidth="sm" sx={{ py: 4, textAlign: 'center' }}>
        <Typography variant="h6" color="error">
          {status}
        </Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 3 }}>
        <CircularProgress size={60} />
        <Typography variant="h6" textAlign="center">
          {status}
        </Typography>
        <Typography variant="body2" textAlign="center" color="text.secondary">
          This should only take a moment...
        </Typography>
      </Box>
    </Container>
  );
};

export default PostLoginHandler;
