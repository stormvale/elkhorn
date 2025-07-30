import React from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  IconButton,
  Paper,
  Typography,
  Chip
} from '@mui/material';
import {
  Add as AddIcon,
  Fastfood as FastfoodIcon,
  Delete as DeleteIcon
} from '@mui/icons-material';
import { RestaurantResponse } from '../api/apiSlice-generated';

interface MenuItemsListProps {
  selectedRestaurant: RestaurantResponse | undefined;
  isDeletingMeal: boolean;
  onAddMeal: () => void;
  onDeleteMeal: (mealId: string) => void;
}

const MenuItemsList: React.FC<MenuItemsListProps> = ({
  selectedRestaurant,
  isDeletingMeal,
  onAddMeal,
  onDeleteMeal
}) => {
  return (
    <Paper sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h6">
          Menu Items
          {selectedRestaurant && (
            <Typography variant="body2" color="text.secondary" component="span" sx={{ ml: 1 }}>
              - {selectedRestaurant.name}
            </Typography>
          )}
        </Typography>
        {selectedRestaurant && (
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={onAddMeal}
          >
            Add Menu Item
          </Button>
        )}
      </Box>

      {selectedRestaurant ? (
        selectedRestaurant.menu.length > 0 ? (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            {selectedRestaurant.menu.map((meal) => (
              <Card key={meal.id} variant="outlined">
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                    <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                      <FastfoodIcon color="primary" sx={{ mr: 1 }} />
                      <Typography variant="h6">
                        {meal.name}
                      </Typography>
                    </Box>
                    <IconButton
                      size="small"
                      onClick={() => onDeleteMeal(meal.id)}
                      disabled={isDeletingMeal}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                  <Typography variant="h6" color="primary" sx={{ mb: 1 }}>
                    ${meal.price.toFixed(2)}
                  </Typography>
                  {meal.availableModifiers.length > 0 && (
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5 }}>
                      {meal.availableModifiers.map((modifier, index) => (
                        <Chip
                          key={index}
                          label={`${modifier.name} (+$${modifier.priceAdjustment.toFixed(2)})`}
                          size="small"
                          variant="outlined"
                        />
                      ))}
                    </Box>
                  )}
                </CardContent>
              </Card>
            ))}
          </Box>
        ) : (
          <Box sx={{ textAlign: 'center', py: 4 }}>
            <FastfoodIcon sx={{ fontSize: 48, color: 'text.secondary', mb: 2 }} />
            <Typography variant="body1" color="text.secondary">
              No menu items yet
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Click "Add Menu Item" to create the first dish
            </Typography>
          </Box>
        )
      ) : (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <Typography variant="body1" color="text.secondary">
            Select a restaurant to view menu items
          </Typography>
        </Box>
      )}
    </Paper>
  );
};

export default MenuItemsList;
