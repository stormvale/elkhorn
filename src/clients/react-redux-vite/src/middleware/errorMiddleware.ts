import { Middleware, isRejectedWithValue } from '@reduxjs/toolkit';
import { showNotification } from '../features/notifications/notificationSlice';

export const errorMiddleware: Middleware =
  ({ dispatch }) =>
  (next) =>
  (action) => {
    if (isRejectedWithValue(action)) {
      const errorPayload = action.payload as { data?: { message?: string }; status?: number };
      const errorMessage = errorPayload.data?.message || 'An unexpected error occurred. Status: ' + errorPayload.status || 'Unknown';
      dispatch(showNotification({ message: errorMessage, severity: 'error' }));
    }
    return next(action);
  };