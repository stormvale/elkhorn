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

  const handleLogout = () => {
    instance.logoutPopup()
      .then(() => dispatch(clearCredentials()))
      .catch(error => console.error('Logout error:', error));
  };

  return (
    <Stack direction="row" spacing={2} alignItems="center">
      <Typography variant="h5">{user_name}</Typography>
      <Button onClick={() => handleLogout()} color="warning">
        Logout
      </Button>
    </Stack>
  );
};