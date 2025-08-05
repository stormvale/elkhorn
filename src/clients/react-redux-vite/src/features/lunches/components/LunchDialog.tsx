import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  Typography,
  Alert
} from '@mui/material';
import { useScheduleLunchMutation } from '../api/apiSlice';
import { UserSchool } from '../../../app/authSlice';
import { LunchResponse, ScheduleLunchRequest } from '../api/apiSlice-generated';

interface Restaurant {
  id: string;
  name: string;
}

interface LunchDialogProps {
  open: boolean;
  lunch?: LunchResponse | null;
  restaurants: Restaurant[];
  currentSchool: UserSchool;
  onClose: () => void;
  onSuccess: () => void;
}

const LunchDialog: React.FC<LunchDialogProps> = ({
  open,
  lunch,
  restaurants,
  currentSchool,
  onClose,
  onSuccess
}) => {
  const [lunchForm, setLunchForm] = useState<ScheduleLunchRequest>({
    schoolId: currentSchool.schoolId,
    restaurantId: '',
    date: ''
  });
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [dateError, setDateError] = useState<string>('');

  const isEditMode = !!lunch;

  // API hooks
  const [scheduleLunch, { isLoading: isScheduling }] = useScheduleLunchMutation();

  // Calculate minimum date (one week from today)
  const getMinDate = () => {
    const today = new Date();
    const minDate = new Date(today);
    minDate.setDate(today.getDate() + 7);
    return minDate.toISOString().split('T')[0]; // Return YYYY-MM-DD format
  };

  // Populate form when editing
  useEffect(() => {
    if (lunch && open) {
      setLunchForm({
        schoolId: lunch.schoolId,
        restaurantId: lunch.restaurantId,
        date: lunch.date
      });
      setSelectedDate(lunch.date);
    } else if (!isEditMode && open) {
      // Reset form for new lunch
      setLunchForm({
        schoolId: currentSchool.schoolId,
        restaurantId: '',
        date: ''
      });
      setSelectedDate('');
    }
  }, [lunch, open, isEditMode, currentSchool]);

  const handleDateChange = (dateValue: string) => {
    setSelectedDate(dateValue);
    setDateError('');
    
    if (dateValue) {
      const selectedDateObj = new Date(dateValue);
      const today = new Date();
      const minDate = new Date(today);
      minDate.setDate(today.getDate() + 7);
      
      if (selectedDateObj <= minDate) {
        setDateError('Lunch must be scheduled at least one week in advance');
        return;
      }
      
      setLunchForm({
        ...lunchForm,
        date: dateValue
      });
    } else {
      setLunchForm({
        ...lunchForm,
        date: ''
      });
    }
  };

  const handleRestaurantChange = (restaurantId: string) => {
    setLunchForm({
      ...lunchForm,
      restaurantId
    });
  };

  const handleSubmit = async () => {
    if (dateError) {
      return;
    }

    try {
      if (isEditMode) {
        // TODO: Implement update lunch when API is available
        console.log('Update lunch not yet implemented');
      } else {
        // Create new lunch
        await scheduleLunch(lunchForm).unwrap();
      }
      
      onSuccess();
    } catch (error) {
      console.error(`Failed to ${isEditMode ? 'update' : 'schedule'} lunch:`, error);
    }
  };

  const handleClose = () => {
    // Reset form when closing
    if (!isEditMode) {
      setLunchForm({
        schoolId: currentSchool.schoolId,
        restaurantId: '',
        date: ''
      });
      setSelectedDate('');
    }
    setDateError('');
    onClose();
  };

  const isFormValid = lunchForm.restaurantId && 
                     lunchForm.date && 
                     !dateError;

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        {isEditMode ? 'Edit Lunch' : 'Schedule New Lunch'}
      </DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 1 }}>
          {/* School (read-only) */}
          <TextField
            label="School"
            value={currentSchool.schoolName}
            fullWidth
            disabled
            helperText="Lunch will be scheduled for the current school"
          />

          {/* Restaurant Selection */}
          <FormControl fullWidth required>
            <InputLabel>Restaurant</InputLabel>
            <Select
              value={lunchForm.restaurantId}
              label="Restaurant"
              onChange={(e) => handleRestaurantChange(e.target.value)}
            >
              {restaurants.length === 0 ? (
                <MenuItem disabled>No restaurants available</MenuItem>
              ) : (
                restaurants.map((restaurant) => (
                  <MenuItem key={restaurant.id} value={restaurant.id}>
                    {restaurant.name}
                  </MenuItem>
                ))
              )}
            </Select>
          </FormControl>

          {/* Date Selection */}
          <TextField
            label="Lunch Date"
            type="date"
            value={selectedDate}
            onChange={(e) => handleDateChange(e.target.value)}
            fullWidth
            required
            error={!!dateError}
            helperText={dateError || 'Must be at least one week in advance'}
            InputLabelProps={{
              shrink: true,
            }}
            inputProps={{
              min: getMinDate()
            }}
          />

          {dateError && (
            <Alert severity="error" sx={{ mt: 1 }}>
              {dateError}
            </Alert>
          )}

          <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
            The lunch will include all available menu items from the selected restaurant.
          </Typography>
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button 
          onClick={handleSubmit} 
          variant="contained"
          disabled={isScheduling || !isFormValid}
        >
          {isScheduling 
            ? (isEditMode ? 'Saving...' : 'Scheduling...') 
            : (isEditMode ? 'Save Changes' : 'Schedule Lunch')
          }
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default LunchDialog;
