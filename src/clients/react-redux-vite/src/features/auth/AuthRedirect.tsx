import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../../app/hooks';
import { AuthUser, setCredentials } from '../../app/authSlice';

const AuthRedirect = () => {
  const navigate = useNavigate();
  const { instance } = useMsal();
  const dispatch = useAppDispatch();

  useEffect(() => {
    const handleRedirect = async () => {
      try {
        const response = await instance.handleRedirectPromise();
        
        if (response && response.account) {
          console.log('Authentication successful:', response);
          instance.setActiveAccount(response.account);
          
          if (response.accessToken) {
            const idTokenClaims = response.account.idTokenClaims ?? {};

            const user: AuthUser = {
              id: idTokenClaims['oid'] ?? '',
              email: response.account.username,
              name: response.account.name ?? '',
              availableSchools: [],
              roles: Array.isArray(idTokenClaims.roles)
                ? idTokenClaims.roles 
                : Array.isArray(idTokenClaims['extension_Roles'])
                  ? idTokenClaims['extension_Roles']
                  : []
            };

            // store credentials in Redux state
            dispatch(setCredentials({ accessToken: response.accessToken, user }));
            navigate('/post-login');
          }
        } else {
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
