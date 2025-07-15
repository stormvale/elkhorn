import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AlertColor } from '@mui/material';

interface NotificationState {
  message: string;
  severity: AlertColor;
  open: boolean;
}

const initialState: NotificationState = {
  message: '',
  severity: 'info',
  open: false
};

export const notificationSlice = createSlice({
  name: 'notification',
  initialState,
  reducers: {
    showNotification: (
      state, 
      action: PayloadAction<{ message: string; severity: AlertColor }>
    ) => {
      state.message = action.payload.message;
      state.severity = action.payload.severity;
      state.open = true;
    },
    hideNotification: (state) => {
      state.open = false;
    }
  }
});

export const { showNotification, hideNotification } = notificationSlice.actions;
export default notificationSlice.reducer;