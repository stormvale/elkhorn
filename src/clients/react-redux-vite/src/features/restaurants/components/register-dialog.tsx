import React, { useState } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
  Box,
  Typography,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  CircularProgress
} from '@mui/material';
import { useRegisterRestaurantMutation } from '../api/apiSlice-generated';
import { RegisterRestaurantRequest } from '../api/apiSlice-generated';
import { useAppDispatch } from '../../../app/hooks';
import { showNotification } from '../../notifications/notificationSlice';

interface RegisterRestaurantDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess: (restaurantId: string) => void;
}

export const RegisterRestaurantDialog: React.FC<RegisterRestaurantDialogProps> = ({
  open,
  onClose,
  onSuccess
}) => {
  const dispatch = useAppDispatch();
  const [registerRestaurant, { isLoading }] = useRegisterRestaurantMutation();

  const [formData, setFormData] = useState<RegisterRestaurantRequest>({
    name: '',
    contact: {
      name: '',
      type: 'Parent',
      email: '',
      phone: ''
    },
    address: {
      street: '',
      city: '',
      state: '',
      postCode: ''
    }
  });

  const handleInputChange = (field: string, value: string) => {
    if (field.startsWith('contact.')) {
      const contactField = field.split('.')[1];
      setFormData(prev => ({
        ...prev,
        contact: {
          ...prev.contact,
          [contactField]: value
        }
      }));
    } else if (field.startsWith('address.')) {
      const addressField = field.split('.')[1];
      setFormData(prev => ({
        ...prev,
        address: {
          ...prev.address,
          [addressField]: value
        }
      }));
    } else {
      setFormData(prev => ({
        ...prev,
        [field]: value
      }));
    }
  };

  const handleSubmit = async () => {
    try {
      const response = await registerRestaurant(formData).unwrap();
      
      dispatch(showNotification({
        message: 'Restaurant registered successfully!',
        severity: 'success'
      }));

      // Reset form
      setFormData({
        name: '',
        contact: {
          name: '',
          type: 'Parent',
          email: '',
          phone: ''
        },
        address: {
          street: '',
          city: '',
          state: '',
          postCode: ''
        }
      });

      onSuccess(response.restaurantId);
      onClose();
    } catch (error) {
      console.error('Failed to register restaurant:', error);
      dispatch(showNotification({
        message: 'Failed to register restaurant',
        severity: 'error'
      }));
    }
  };

  const handleClose = () => {
    if (!isLoading) {
      onClose();
    }
  };

  const isFormValid = () => {
    return (
      formData.name.trim() !== '' &&
      formData.contact.name.trim() !== '' &&
      formData.contact.email.trim() !== '' &&
      formData.address.street.trim() !== '' &&
      formData.address.city.trim() !== '' &&
      formData.address.state.trim() !== '' &&
      formData.address.postCode.trim() !== ''
    );
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Register New Restaurant</DialogTitle>
      
      <DialogContent>
        <Box sx={{ pt: 2 }}>
          <Box 
            sx={{ 
              display: 'grid', 
              gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)'},
              gap: 3
            }}
          >
            {/* Restaurant Name */}
            <TextField
              fullWidth
              label="Restaurant Name"
              value={formData.name}
              onChange={(e) => handleInputChange('name', e.target.value)}
              required
              disabled={isLoading}
              sx={{ gridColumn: '1 / -1' }} // Full width for restaurant name
            />

            {/* Contact Information */}
            <Box sx={{ gridColumn: '1 / -1' }}>
              <Typography variant="h6" gutterBottom>
                Contact Information
              </Typography>
            </Box>

            <TextField
              fullWidth
              label="Contact Name"
              value={formData.contact.name}
              onChange={(e) => handleInputChange('contact.name', e.target.value)}
              required
              disabled={isLoading}
            />

              <FormControl fullWidth disabled={isLoading}>
                <InputLabel>Contact Type</InputLabel>
                <Select
                  value={formData.contact.type}
                  label="Contact Type"
                  onChange={(e) => handleInputChange('contact.type', e.target.value)}
                >
                  <MenuItem value="Primary">Primary</MenuItem>
                  <MenuItem value="Manager">Manager</MenuItem>
                  <MenuItem value="Owner">Owner</MenuItem>
                  <MenuItem value="Assistant">Assistant</MenuItem>
                </Select>
              </FormControl>

              <TextField
                fullWidth
                label="Email"
                type="email"
                value={formData.contact.email}
                onChange={(e) => handleInputChange('contact.email', e.target.value)}
                required
                disabled={isLoading}
              />

              <TextField
                fullWidth
                label="Phone"
                value={formData.contact.phone}
                onChange={(e) => handleInputChange('contact.phone', e.target.value)}
                disabled={isLoading}
              />

            {/* Address Information */}
            <Box sx={{ gridColumn: '1 / -1' }}>
              <Typography variant="h6" gutterBottom>
                Address Information
              </Typography>
            </Box>

              <TextField
                fullWidth
                label="Street Address"
                value={formData.address.street}
                onChange={(e) => handleInputChange('address.street', e.target.value)}
                required
                disabled={isLoading}
              />

              <TextField
                fullWidth
                label="City"
                value={formData.address.city}
                onChange={(e) => handleInputChange('address.city', e.target.value)}
                required
                disabled={isLoading}
              />

              <TextField
                fullWidth
                label="State"
                value={formData.address.state}
                onChange={(e) => handleInputChange('address.state', e.target.value)}
                required
                disabled={isLoading}
              />

              <TextField
                fullWidth
                label="Post Code"
                value={formData.address.postCode}
                onChange={(e) => handleInputChange('address.postCode', e.target.value)}
                required
                disabled={isLoading}
              />
          </Box>
        </Box>
      </DialogContent>

      <DialogActions>
        <Button onClick={handleClose} disabled={isLoading}>
          Cancel
        </Button>
        <Button
          onClick={handleSubmit}
          variant="contained"
          disabled={!isFormValid() || isLoading}
          startIcon={isLoading ? <CircularProgress size={20} /> : null}
        >
          {isLoading ? 'Registering...' : 'Register Restaurant'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};