import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';

const AuthRedirect = () => {
  const navigate = useNavigate();
  const { instance } = useMsal();

  useEffect(() => {
    const handleRedirect = async () => {
      try {
        // Handle the redirect response
        const response = await instance.handleRedirectPromise();
        
        if (response && response.account) {
          console.log('Authentication successful:', response);
          
          // Set the active account
          instance.setActiveAccount(response.account);
          
          // Navigate to post-login handler to check user profile and set up context
          navigate('/post-login');
        } else {
          // No response means we're probably not coming from a redirect
          console.log('No redirect response, navigating to auth landing');
          navigate('/');
        }
      } catch (error) {
        console.error('Authentication redirect error:', error);
        navigate('/');
      }
    };

    handleRedirect();
  }, [navigate, instance]);

  return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      height: '100vh',
      flexDirection: 'column'
    }}>
      <h2>Processing authentication...</h2>
      <p>Please wait while we complete your sign-in.</p>
    </div>
  );
};

export default AuthRedirect;
