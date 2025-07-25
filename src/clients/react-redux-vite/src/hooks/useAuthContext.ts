import { useSelector } from 'react-redux';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../app/hooks';
import { RootState } from '../app/store';
import { clearCredentials } from '../app/authSlice';

export const useAuthContext = () => {
  const { instance } = useMsal();
  const dispatch = useAppDispatch();
  const { isAuthenticated, currentUser, currentSchool } = useSelector((state: RootState) => state.auth);

  const logout = async () => {
    try {
      dispatch(clearCredentials());
      sessionStorage.removeItem('schoolId');
      await instance.logoutRedirect({ postLogoutRedirectUri: '/' });
    } catch (error) {
      console.error('Logout failed:', error);
      throw error;
    }
  };

  return {
    isAuthenticated,
    currentUser,
    currentSchool,
    availableSchools: currentUser?.availableSchools || [],
    logout
  };
}