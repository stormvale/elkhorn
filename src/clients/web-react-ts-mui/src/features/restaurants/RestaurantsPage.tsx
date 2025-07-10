import { useEffect } from 'react';
import { Box, Button, CircularProgress, Typography } from '@mui/material';
import { RootState, AppDispatch } from '../../app/store';
import { useDispatch, useSelector } from 'react-redux';
import { fetchRestaurants, addMeal, removeMeal, setSelectedRestaurant } from './restaurantsSlice';
import RestaurantDetails from './components/RestaurantDetails';
import RestaurantList from './components/RestaurantsList';

const RestaurantsPage: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { list, selected, loading } = useSelector((state: RootState) => state.restaurants);

  useEffect(() => {
    dispatch(fetchRestaurants());
  }, [dispatch]);

  const handleSelect = (restaurant: any) => {
    dispatch(setSelectedRestaurant(restaurant));
  };

  const handleAddRestaurant = () => {
    console.log('TODO: Add restaurant form');
  };

  const handleAddMeal = () => {
    dispatch(addMeal({
      id: crypto.randomUUID(),
      name: 'New Meal',
      price: 9.99,
      availableModifiers: [],
    }));
  };

  const handleRemoveMeal = (id: string) => {
    dispatch(removeMeal(id));
  };

  return (
    <Box sx={{ display: 'flex', gap: 4, p: 2 }}>
      <Box sx={{ flex: 1 }}>
        <Typography variant="h4" gutterBottom>Restaurants</Typography>

        {loading ? (
          <CircularProgress sx={{ display: 'block', margin: '2rem auto' }} />
        ) : (
          <>
            <Button variant="contained" onClick={handleAddRestaurant}>Add Restaurant</Button>
            <RestaurantList restaurants={list} onSelect={handleSelect} />
          </>
        )}

      </Box>

      <Box sx={{ flex: 2 }}>
        <RestaurantDetails
          restaurant={selected}
          onAddMeal={handleAddMeal}
          onRemoveMeal={handleRemoveMeal}
        />
      </Box>
    </Box>
  );
};

export default RestaurantsPage;