import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useMsal } from '@azure/msal-react';
import { tokenStorage } from '../../utils/tokenStorage';
import { restoreAuthStateFromLocalStorage, setCredentials } from '../../app/authSlice';


export const useAuthInit = () => {
  const dispatch = useDispatch();
  const { instance, accounts } = useMsal();

  useEffect(() => {
    const initializeAuth = async () => {
      try {
        // Check if we have valid stored auth data
        const storedToken = tokenStorage.getAccessToken();
        const storedUser = tokenStorage.getUser();

        if (storedToken && storedUser && tokenStorage.isTokenValid()) {
          dispatch(restoreAuthStateFromLocalStorage());
        } else if (accounts.length > 0) {
          // User is logged in with MSAL but we don't have valid stored data
          // This could happen after a browser refresh
          try {
            const silentRequest = {
              scopes: ["User.Read"],
              account: accounts[0]
            };
            
            const response = await instance.acquireTokenSilent(silentRequest);
            // Handle successful silent token acquisition
            // we would need to dispatch setCredentials here with the new token
          } catch (error) {
            console.error('Silent token acquisition failed:', error);
            tokenStorage.clearAuthData();
          }
        } else {
          // No valid auth data anywhere
          tokenStorage.clearAuthData();
        }
      } catch (error) {
        console.error('Auth initialization error:', error);
        tokenStorage.clearAuthData();
      }
    };

    initializeAuth();
  }, [dispatch, instance, accounts]);
};