import { useSelector } from 'react-redux';
import { Button, Stack, Typography } from '@mui/material';
import { RootState } from '../../app/store';
import { useAppDispatch } from '../../app/hooks';
import { useMsal } from '@azure/msal-react';
import { clearCredentials } from '../../app/authSlice';

export const HeaderLogout = () => {
  const dispatch = useAppDispatch()
  const { instance } = useMsal();
  const user_name = useSelector((state: RootState) => state.auth.user?.name);
  const isAuthenticated = useSelector((state: RootState) => state.auth.isAuthenticated);

  if (!isAuthenticated) {
    return null; // Don't render if not authenticated
  }
  
  const handleLogoutPopup = () => {
    instance.logout()
      .then(() => {
        dispatch(clearCredentials());
      }).catch((error) => console.error(error));
  };

  return (
    <Stack direction="row" spacing={2} alignItems="center">
      <Typography variant="h5">{user_name}</Typography>
      <Button onClick={() => handleLogoutPopup()} color="warning">
        Logout
      </Button>
    </Stack>
  );
};