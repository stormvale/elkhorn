import { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useMsal } from '@azure/msal-react';
import { tokenStorage } from '../../utils/tokenStorage';
import { restoreAuthStateFromLocalStorage, setCredentials } from '../../app/authSlice';
import { MSALTokenClaims, User } from '../../types';

export const useAuthInit = () => {
  const dispatch = useDispatch();
  const { instance, accounts } = useMsal();

  // Helper function to create User object from MSAL account and claims
  const createUserFromMSAL = (account: any, claims?: MSALTokenClaims): User => {
    return {
      id: claims?.oid || account.localAccountId || '',
      email: claims?.email || account.username || '',
      name: claims?.name || account.name || '',
      username: claims?.preferred_username || account.username || '',
      roles: claims?.roles || [],
      tenantId: claims?.tid || account.tenantId,
    };
  };

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
            const account = response.account || accounts[0];
            const claims = response.idTokenClaims as MSALTokenClaims;
            const user = createUserFromMSAL(account, claims);

            dispatch(setCredentials({
              accessToken: response.accessToken,
              user: user
            }));
          } catch (error) {
            console.error('Silent token acquisition failed:', error);
            tokenStorage.clearAuthData();
          }
        } else {
          tokenStorage.clearAuthData(); // No valid auth data anywhere
        }
      } catch (error) {
        console.error('Auth initialization error:', error);
        tokenStorage.clearAuthData();
      }
    };

    initializeAuth();
  }, [dispatch, instance, accounts]);
};