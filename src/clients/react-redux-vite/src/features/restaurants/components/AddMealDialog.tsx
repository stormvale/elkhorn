import React from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField
} from '@mui/material';
import { CreateMealRequest } from '../api/apiSlice-generated';
import MealModifierForm from './MealModifierForm';

interface MealModifier {
  name: string;
  priceAdjustment: number;
}

interface AddMealDialogProps {
  open: boolean;
  isCreatingMeal: boolean;
  mealForm: CreateMealRequest;
  modifiers: MealModifier[];
  onClose: () => void;
  onSubmit: () => void;
  onMealFormChange: (form: CreateMealRequest) => void;
  onModifiersChange: (modifiers: MealModifier[]) => void;
}

const AddMealDialog: React.FC<AddMealDialogProps> = ({
  open,
  isCreatingMeal,
  mealForm,
  modifiers,
  onClose,
  onSubmit,
  onMealFormChange,
  onModifiersChange
}) => {
  return (
    <Dialog open={open} onClose={onClose} maxWidth="sm" fullWidth>
      <DialogTitle>Add Menu Item</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 1 }}>
          <TextField
            label="Meal Name"
            value={mealForm.name}
            onChange={(e) => onMealFormChange({ ...mealForm, name: e.target.value })}
            fullWidth
          />
          <TextField
            label="Price"
            type="number"
            value={mealForm.price}
            onChange={(e) => onMealFormChange({ ...mealForm, price: parseFloat(e.target.value) || 0 })}
            fullWidth
            inputProps={{ min: 0, step: 0.01 }}
          />
          
          <MealModifierForm
            modifiers={modifiers}
            onModifiersChange={onModifiersChange}
          />
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button 
          onClick={onSubmit} 
          variant="contained"
          disabled={isCreatingMeal || !mealForm.name.trim() || mealForm.price <= 0}
        >
          {isCreatingMeal ? 'Adding...' : 'Add Menu Item'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default AddMealDialog;
