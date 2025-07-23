import { useSelector } from 'react-redux';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../app/hooks';
import { RootState } from '../app/store';
import { clearCredentials, setCurrentSchool } from '../app/authSlice';
import { UserSchoolDto } from '../features/users/api/apiSlice-generated';

/**
 * Hook to get the current authenticated user and auth status
 */
export const useAuthenticatedUser = () => {
  const { user, isAuthenticated } = useSelector((state: RootState) => state.auth);
  return { user, isAuthenticated };
};

/**
 * Hook to get school context (current school and available schools)
 */
export const useSchoolContext = () => {
  const dispatch = useAppDispatch();
  const { currentSchool, availableSchools } = useSelector((state: RootState) => state.auth);
  
  const switchSchool = (school: UserSchoolDto) => {
    dispatch(setCurrentSchool(school));
  };
  
  return { 
    currentSchool, 
    availableSchools, 
    switchSchool,
    hasMultipleSchools: availableSchools.length > 1
  };
};

/**
 * Hook for logout functionality
 */
export const useLogout = () => {
  const { instance } = useMsal();
  const dispatch = useAppDispatch();
  
  const logout = async () => {
    try {
      // Clear Redux state
      dispatch(clearCredentials());
      
      // Clear session storage
      sessionStorage.removeItem('schoolId');
      
      // Logout from MSAL
      await instance.logoutRedirect({ postLogoutRedirectUri: '/' });
    } catch (error) {
      console.error('Logout failed:', error);
      throw error;
    }
  };
  
  return { logout };
};

/**
 * Main hook that combines auth and school context (replacement for useAppContext)
 */
export const useApp = () => {
  const auth = useAuthenticatedUser();
  const school = useSchoolContext();
  const { logout } = useLogout();
  
  return {
    ...auth,
    ...school,
    logout
  };
};
