import React, { JSX } from 'react';
import { Alert, Snackbar } from '@mui/material';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import { useAppDispatch } from '../../app/hooks';
import { hideNotification } from './notificationSlice';

const NotificationSnackbar = (): JSX.Element => {
  const dispatch = useAppDispatch();
  const notification = useSelector((state: RootState) => state.notification);

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    dispatch(hideNotification());
  };

  return (
    <Snackbar
      open={notification.open}
      autoHideDuration={3000}
      onClose={handleClose}
      anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
    >
      <Alert onClose={handleClose} severity={notification.severity} sx={{ width: '100%' }}>
        {notification.message}
      </Alert>
    </Snackbar>
  );
};

export default NotificationSnackbar;