import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../../app/hooks';
import { setCredentials } from '../../app/authSlice';

const AuthRedirect = () => {
  const navigate = useNavigate();
  const { instance } = useMsal();
  const dispatch = useAppDispatch();

  useEffect(() => {
    const handleRedirect = async () => {
      try {
        // Handle the redirect response
        const response = await instance.handleRedirectPromise();
        
        if (response && response.account) {
          console.log('Authentication successful:', response);
          
          // Set the active account
          instance.setActiveAccount(response.account);
          
          // Store credentials in Redux state
          if (response.accessToken) {
            const user = {
              id: response.account.localAccountId,
              username: response.account.username || response.account.name || '',
              email: response.account.username,
              name: response.account.name || '',
              roles: Array.isArray(response.account.idTokenClaims?.roles) 
                ? response.account.idTokenClaims.roles 
                : Array.isArray(response.account.idTokenClaims?.['extension_Roles'])
                  ? response.account.idTokenClaims['extension_Roles']
                  : []
            };
            
            console.log('Storing credentials in Redux:', { 
              hasToken: !!response.accessToken, 
              user: user.username,
              roles: user.roles
            });
            
            dispatch(setCredentials({
              accessToken: response.accessToken,
              user
            }));
          }
          
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
  }, [navigate, instance, dispatch]);

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
