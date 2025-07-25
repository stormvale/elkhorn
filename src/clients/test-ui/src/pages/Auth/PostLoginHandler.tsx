import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { Container, Typography, CircularProgress, Box } from '@mui/material';
import { userService, UserProfile } from '../../services/userService';
import { schoolContextService } from '../../services/schoolContextService';
import { apiRequest } from '../../../msalConfig';

const PostLoginHandler = () => {
  const { instance, accounts } = useMsal();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(true);
  const [status, setStatus] = useState('Checking your profile...');

  useEffect(() => {
    const handlePostLogin = async () => {
      if (accounts.length === 0) {
        navigate('/');
        return;
      }

      try {
        setStatus('Getting access token...');
        
        // Get access token for API calls
        const tokenResponse = await instance.acquireTokenSilent({
          ...apiRequest,
          account: accounts[0]
        });

        setStatus('Fetching your profile...');
        
        try {
          // Try to get existing user profile
          const userProfile: UserProfile = await userService.getUserProfile(tokenResponse.accessToken);
          
          if (userProfile.schools && userProfile.schools.length > 0) {
            // User has schools linked - set up context with first school
            setStatus('Setting up your school context...');
            
            // Initialize school context service with user's schools
            await schoolContextService.initialize();
            
            // For now, we'll use the first school as the current context
            // You can later add logic to remember the last selected school
            const firstSchool = userProfile.schools[0];
            sessionStorage.setItem('schoolId', firstSchool.id);
            
            // Navigate to home
            navigate('/home');
          } else {
            // User has no schools linked - go to school selector
            setStatus('Welcome! Let\'s get you connected to a school...');
            setTimeout(() => navigate('/school-selector'), 1000);
          }
        } catch (profileError) {
          // User profile doesn't exist yet (new user) - go to school selector
          console.log('New user - no profile found:', profileError);
          setStatus('Welcome! Let\'s get you set up...');
          setTimeout(() => navigate('/school-selector'), 1000);
        }
        
      } catch (error) {
        console.error('Post-login flow failed:', error);
        setStatus('Something went wrong. Please try again.');
        setTimeout(() => navigate('/'), 2000);
      } finally {
        setIsLoading(false);
      }
    };

    handlePostLogin();
  }, [accounts, instance, navigate]);

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
