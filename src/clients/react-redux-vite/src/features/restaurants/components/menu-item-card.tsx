import { 
  Card, 
  CardContent, 
  IconButton, 
  Typography, 
  Box, 
  Chip,
  Stack 
} from '@mui/material';
import { Delete } from '@mui/icons-material';
import { RestaurantMealResponse } from '../api/apiSlice-generated';

interface MenuItemCardProps {
  item: RestaurantMealResponse;
  onDelete: (id: string) => void;
  onModifierRemove: (menuItemId: string, modifierName: string) => void; // Note: using name since modifiers don't have IDs
}

export const MenuItemCard = ({ item, onDelete, onModifierRemove }: MenuItemCardProps) => {
  const handleDelete = () => {
    onDelete(item.id);
  };

  const handleModifierDelete = (modifierName: string) => {
    onModifierRemove(item.id, modifierName);
  };

  return (
    <Card sx={{ position: 'relative', height: '100%' }}>
      <IconButton
        onClick={handleDelete}
        color="error"
        aria-label="delete menu item"
        sx={{ position: 'absolute', top: 8, right: 8 }}
      >
        <Delete />
      </IconButton>
      
      <CardContent sx={{ pr: 6 }}>
        <Typography variant="h6" gutterBottom>
          {item.name}
        </Typography>
        
        <Typography variant="body2" color="text.secondary" gutterBottom>
          ${item.price.toFixed(2)}
        </Typography>

        {/* Available Modifiers Section */}
        {item.availableModifiers && item.availableModifiers.length > 0 && (
          <Box>
            <Typography variant="body2" color="text.secondary" gutterBottom>
              Available Modifiers:
            </Typography>
            <Stack direction="row" spacing={1} sx={{ flexWrap: 'wrap', gap: 1, mt: 1 }}>
              {item.availableModifiers.map((modifier) => (
                <Chip
                  key={modifier.name}
                  label={`${modifier.name} (${modifier.priceAdjustment >= 0 ? '+' : ''}$${modifier.priceAdjustment.toFixed(2)})`}
                  variant="outlined"
                  size="small"
                  onDelete={() => handleModifierDelete(modifier.name)}
                />
              ))}
            </Stack>
          </Box>
        )}
      </CardContent>
    </Card>
  );
};

export default MenuItemCard;