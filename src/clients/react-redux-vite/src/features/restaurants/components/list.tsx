import { Paper, Stack, Typography } from '@mui/material';
import { useListRestaurantsQuery } from '../api/apiSlice';
import { RestaurantResponse } from '../api/apiSlice-generated';
import { useEffect, useState } from 'react';

interface RestaurantListProps {
  onSelected: (id: string | null) => void;
  selectedId: string | null;
}

export const RestaurantList = ({ onSelected, selectedId }: RestaurantListProps) => {
  const { data = [] } = useListRestaurantsQuery();
  //const [selectedIndex, setSelectedIndex] = useState<number | null>(null);

  // Clear selection if selected item no longer exists in the list
  useEffect(() => {
    if (selectedId && !data.find(restaurant => restaurant.id === selectedId)) {
        onSelected(null);
    }
  }, [data, selectedId, onSelected]);

  const handleItemClick = (restaurantId: string) => {
    // Toggle selection: if already selected, deselect; otherwise select
    onSelected(selectedId === restaurantId ? null : restaurantId);
  };

  return (
    <Stack spacing={2}>
      {data.map((restaurant: RestaurantResponse, index) => (
        <Paper
          key={restaurant.id}
          elevation={1}
          sx={{
            padding: 2,
            cursor: 'pointer',
            backgroundColor: selectedId === restaurant.id ? 'primary.light' : 'background.paper',
            borderColor: 'primary.main',
            borderLeft: selectedId === restaurant.id ? 3 : 0,
            transition: 'all 0.2s ease-in-out',
            '&:hover': {
              elevation: 2,
              backgroundColor: selectedId === restaurant.id ? 'primary.light' : 'action.hover',
            }
          }}
          onClick={() => handleItemClick(restaurant.id)}
        >
          <Typography variant="h6">{restaurant.name}</Typography>
          <Typography variant="body2" color="text.secondary">
            {restaurant.address?.city}
          </Typography>
        </Paper>
      ))}

      {data.length === 0 && (
        <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'center', py: 4 }}>
          No restaurants found
        </Typography>
      )}
    </Stack>
  );
};