import React from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Typography
} from '@mui/material';
import { RegisterRestaurantRequest } from '../api/apiSlice-generated';

interface AddRestaurantDialogProps {
  open: boolean;
  isRegistering: boolean;
  restaurantForm: RegisterRestaurantRequest;
  onClose: () => void;
  onSubmit: () => void;
  onFormChange: (form: RegisterRestaurantRequest) => void;
}

const AddRestaurantDialog: React.FC<AddRestaurantDialogProps> = ({
  open,
  isRegistering,
  restaurantForm,
  onClose,
  onSubmit,
  onFormChange
}) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add New Restaurant</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 1 }}>
          <TextField
            label="Restaurant Name"
            value={restaurantForm.name}
            onChange={(e) => onFormChange({ ...restaurantForm, name: e.target.value })}
            fullWidth
          />
          
          <Typography variant="subtitle2" sx={{ mt: 2 }}>Address</Typography>
          <TextField
            label="Street"
            value={restaurantForm.address.street}
            onChange={(e) => onFormChange({ 
              ...restaurantForm, 
              address: { ...restaurantForm.address, street: e.target.value }
            })}
            fullWidth
          />
          <Box sx={{ display: 'flex', gap: 2 }}>
            <TextField
              label="City"
              value={restaurantForm.address.city}
              onChange={(e) => onFormChange({ 
                ...restaurantForm, 
                address: { ...restaurantForm.address, city: e.target.value }
              })}
              fullWidth
            />
            <TextField
              label="State"
              value={restaurantForm.address.state}
              onChange={(e) => onFormChange({ 
                ...restaurantForm, 
                address: { ...restaurantForm.address, state: e.target.value }
              })}
              fullWidth
            />
          </Box>
          <TextField
            label="Post Code"
            value={restaurantForm.address.postCode}
            onChange={(e) => onFormChange({ 
              ...restaurantForm, 
              address: { ...restaurantForm.address, postCode: e.target.value }
            })}
            fullWidth
          />

          <Typography variant="subtitle2" sx={{ mt: 2 }}>Contact Information</Typography>
          <TextField
            label="Contact Name"
            value={restaurantForm.contact.name}
            onChange={(e) => onFormChange({ 
              ...restaurantForm, 
              contact: { ...restaurantForm.contact, name: e.target.value }
            })}
            fullWidth
          />
          <TextField
            label="Email"
            type="email"
            value={restaurantForm.contact.email}
            onChange={(e) => onFormChange({ 
              ...restaurantForm, 
              contact: { ...restaurantForm.contact, email: e.target.value }
            })}
            fullWidth
          />
          <TextField
            label="Phone"
            value={restaurantForm.contact.phone}
            onChange={(e) => onFormChange({ 
              ...restaurantForm, 
              contact: { ...restaurantForm.contact, phone: e.target.value }
            })}
            fullWidth
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button 
          onClick={onSubmit} 
          variant="contained"
          disabled={isRegistering || !restaurantForm.name.trim()}
        >
          {isRegistering ? 'Adding...' : 'Add Restaurant'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AddRestaurantDialog;
