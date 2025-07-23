import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { Container, Typography, CircularProgress, Box } from '@mui/material';
import { schoolContextService } from '../../services/schoolContextService';
import { useProfileQuery } from '../users/api/apiSlice';

const PostLoginHandler = () => {
  const { accounts } = useMsal();
  const navigate = useNavigate();
  const [status, setStatus] = useState('Checking your profile...');

  // This will auto-fetch when component mounts
  const { data: userProfile, isLoading, error } = useProfileQuery();

  console.log('PostLoginHandler render:', { 
    userProfile: !!userProfile, 
    isLoading, 
    error: error ? 'present' : 'none',
    accountsLength: accounts.length 
  });

  useEffect(() => {
    console.log('PostLoginHandler accounts effect:', accounts.length);
    if (accounts.length === 0) {
      navigate('/');
      return;
    }
  }, [accounts, navigate]);

  // Handle the profile query results in a separate useEffect
  useEffect(() => {
    console.log('PostLoginHandler profile effect:', { 
      isLoading, 
      hasError: !!error, 
      hasProfile: !!userProfile 
    });

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
      console.log('User profile loaded:', userProfile);
      
      if (userProfile.schools && userProfile.schools.length > 0) {
        setStatus('Setting up your school context...');
        
        const setupSchoolContext = async () => {
          try {
            await schoolContextService.initialize();
            
            // use the first school as the current context
            const firstSchool = userProfile.schools[0];
            sessionStorage.setItem('schoolId', firstSchool.id);
            
            navigate('/home');
          } catch (error) {
            setStatus('Failed to set up school context');
            setTimeout(() => navigate('/school-selector'), 1000);
          }
        };

        setupSchoolContext();
      } else {
        // User has no schools linked - go to school selector
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
