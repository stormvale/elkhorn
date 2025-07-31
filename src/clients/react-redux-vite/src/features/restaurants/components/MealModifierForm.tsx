import React from 'react';
import {
  Box,
  Button,
  IconButton,
  TextField,
  Typography
} from '@mui/material';
import {
  Add as AddIcon,
  Remove as RemoveIcon
} from '@mui/icons-material';

interface MealModifier {
  name: string;
  priceAdjustment: number;
}

interface MealModifierFormProps {
  modifiers: MealModifier[];
  onModifiersChange: (modifiers: MealModifier[]) => void;
}

const MealModifierForm: React.FC<MealModifierFormProps> = ({
  modifiers,
  onModifiersChange
}) => {
  const handleAddModifier = () => {
    onModifiersChange([...modifiers, { name: '', priceAdjustment: 0 }]);
  };

  const handleRemoveModifier = (index: number) => {
    onModifiersChange(modifiers.filter((_, i) => i !== index));
  };

  const handleModifierChange = (index: number, field: 'name' | 'priceAdjustment', value: string | number) => {
    const updated = [...modifiers];
    updated[index] = { ...updated[index], [field]: value };
    onModifiersChange(updated);
  };

  return (
    <Box sx={{ mt: 2 }}>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
        <Typography variant="h6">Meal Modifiers</Typography>
        <Button
          size="small"
          startIcon={<AddIcon />}
          onClick={handleAddModifier}
          variant="outlined"
        >
          Add Modifier
        </Button>
      </Box>
      
      {modifiers.length === 0 ? (
        <Typography variant="body2" color="text.secondary">
          No modifiers added yet
        </Typography>
      ) : (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          {modifiers.map((modifier, index) => (
            <Box
              key={index}
              sx={{
                display: 'flex',
                gap: 1,
                alignItems: 'center',
                p: 2,
                border: '1px solid',
                borderColor: 'divider',
                borderRadius: 1
              }}
            >
              <TextField
                label="Modifier Name"
                value={modifier.name}
                onChange={(e) => handleModifierChange(index, 'name', e.target.value)}
                size="small"
                sx={{ flex: 1 }}
              />
              <TextField
                label="Price Adjustment"
                type="number"
                value={modifier.priceAdjustment}
                onChange={(e) => handleModifierChange(index, 'priceAdjustment', parseFloat(e.target.value) || 0)}
                size="small"
                sx={{ width: 150 }}
                inputProps={{ step: 0.01 }}
              />
              <IconButton
                onClick={() => handleRemoveModifier(index)}
                color="error"
                size="small"
              >
                <RemoveIcon />
              </IconButton>
            </Box>
          ))}
        </Box>
      )}
    </Box>
  );
};

export default MealModifierForm;
