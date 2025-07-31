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
  Restaurant as RestaurantIcon,
  Delete as DeleteIcon
} from '@mui/icons-material';
import { RestaurantResponse } from '../api/apiSlice-generated';

interface RestaurantListProps {
  restaurants: RestaurantResponse[];
  selectedRestaurantId: string | null;
  isDeleting: boolean;
  onSelectRestaurant: (restaurantId: string) => void;
  onAddRestaurant: () => void;
  onDeleteRestaurant: (restaurantId: string) => void;
}

const RestaurantList: React.FC<RestaurantListProps> = ({
  restaurants,
  selectedRestaurantId,
  isDeleting,
  onSelectRestaurant,
  onAddRestaurant,
  onDeleteRestaurant
}) => {
  return (
    <Paper sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h6">
          Restaurants
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={onAddRestaurant}
        >
          Add Restaurant
        </Button>
      </Box>

      {restaurants.length > 0 ? (
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          {restaurants.map((restaurant) => (
            <Card 
              key={restaurant.id} 
              variant={selectedRestaurantId === restaurant.id ? "outlined" : "elevation"}
              sx={{ 
                cursor: 'pointer',
                border: selectedRestaurantId === restaurant.id ? 2 : 1,
                borderColor: selectedRestaurantId === restaurant.id ? 'primary.main' : 'divider'
              }}
              onClick={() => onSelectRestaurant(restaurant.id)}
            >
              <CardContent>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
                    <RestaurantIcon color="primary" sx={{ mr: 1 }} />
                    <Typography variant="h6">
                      {restaurant.name}
                    </Typography>
                  </Box>
                  <IconButton
                    size="small"
                    onClick={(e) => {
                      e.stopPropagation();
                      onDeleteRestaurant(restaurant.id);
                    }}
                    disabled={isDeleting}
                  >
                    <DeleteIcon />
                  </IconButton>
                </Box>
                <Typography variant="body2" color="text.secondary">
                  {restaurant.address.street}, {restaurant.address.city}
                </Typography>
                <Chip 
                  label={`${restaurant.menu.length} menu items`} 
                  size="small" 
                  sx={{ mt: 1 }}
                />
              </CardContent>
            </Card>
          ))}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <RestaurantIcon sx={{ fontSize: 48, color: 'text.secondary', mb: 2 }} />
          <Typography variant="body1" color="text.secondary">
            No restaurants registered yet
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Click "Add Restaurant" to get started
          </Typography>
        </Box>
      )}
    </Paper>
  );
};

export default RestaurantList;
