import React, { useState } from 'react';
import { Container, Typography, Box, Alert } from '@mui/material';
import { 
  useListRestaurantsQuery, 
  useRegisterRestaurantMutation, 
  useDeleteRestaurantMutation,
  useCreateRestaurantMealMutation,
  useDeleteRestaurantMealMutation,
  RegisterRestaurantRequest,
  CreateMealRequest
} from './api/apiSlice-generated';
import RestaurantList from './components/RestaurantList';
import MenuItemsList from './components/MenuItemsList';
import AddRestaurantDialog from './components/AddRestaurantDialog';
import AddMealDialog from './components/AddMealDialog';

const RestaurantsPage: React.FC = () => {
  const [selectedRestaurantId, setSelectedRestaurantId] = useState<string | null>(null);
  const [showAddRestaurantDialog, setShowAddRestaurantDialog] = useState(false);
  const [showAddMealDialog, setShowAddMealDialog] = useState(false);

  // API hooks
  const { data: restaurants = [], isLoading, error } = useListRestaurantsQuery();
  const [registerRestaurant, { isLoading: isRegistering }] = useRegisterRestaurantMutation();
  const [deleteRestaurant, { isLoading: isDeleting }] = useDeleteRestaurantMutation();
  const [createMeal, { isLoading: isCreatingMeal }] = useCreateRestaurantMealMutation();
  const [deleteMeal, { isLoading: isDeletingMeal }] = useDeleteRestaurantMealMutation();

  // Form states
  const [restaurantForm, setRestaurantForm] = useState<RegisterRestaurantRequest>({
    name: '',
    address: {
      street: '',
      city: '',
      postCode: '',
      state: ''
    },
    contact: {
      name: '',
      email: '',
      phone: '',
      type: 'Manager'
    }
  });

  const [mealForm, setMealForm] = useState<CreateMealRequest>({
    name: '',
    price: 0,
    modifiers: []
  });

  // Track modifiers being edited
  const [currentModifiers, setCurrentModifiers] = useState<Array<{
    name: string;
    priceAdjustment: number;
  }>>([]);

  const selectedRestaurant = restaurants.find(r => r.id === selectedRestaurantId);

  const handleAddRestaurant = async () => {
    try {
      await registerRestaurant(restaurantForm).unwrap();
      setShowAddRestaurantDialog(false);
      // Reset form
      setRestaurantForm({
        name: '',
        address: { street: '', city: '', postCode: '', state: '' },
        contact: { name: '', email: '', phone: '', type: 'Manager' }
      });
    } catch (error) {
      console.error('Failed to register restaurant:', error);
    }
  };

  const handleDeleteRestaurant = async (restaurantId: string) => {
    if (confirm('Are you sure you want to delete this restaurant?')) {
      try {
        await deleteRestaurant(restaurantId).unwrap();
        if (selectedRestaurantId === restaurantId) {
          setSelectedRestaurantId(null);
        }
      } catch (error) {
        console.error('Failed to delete restaurant:', error);
      }
    }
  };

  const handleAddMeal = async () => {
    if (!selectedRestaurantId) return;
    
    try {
      // Update meal form with current modifiers
      const updatedMealForm = {
        ...mealForm,
        modifiers: currentModifiers
      };
      
      await createMeal({
        restaurantId: selectedRestaurantId,
        createMealRequest: updatedMealForm
      }).unwrap();
      setShowAddMealDialog(false);
      // Reset form
      setMealForm({ name: '', price: 0, modifiers: [] });
      setCurrentModifiers([]);
    } catch (error) {
      console.error('Failed to create meal:', error);
    }
  };

  const handleDeleteMeal = async (mealId: string) => {
    if (!selectedRestaurantId) return;
    
    if (confirm('Are you sure you want to delete this meal?')) {
      try {
        await deleteMeal({
          restaurantId: selectedRestaurantId,
          mealId: mealId
        }).unwrap();
      } catch (error) {
        console.error('Failed to delete meal:', error);
      }
    }
  };

  if (isLoading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Typography>Loading restaurants...</Typography>
      </Container>
    );
  }

  if (error) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">Failed to load restaurants</Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Restaurant Management
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Manage restaurants and their menu items
        </Typography>
      </Box>

      <Box sx={{ display: 'flex', gap: 3, flexDirection: { xs: 'column', md: 'row' } }}>
        {/* Restaurant List */}
        <Box sx={{ flex: { md: 0.4 } }}>
          <RestaurantList
            restaurants={restaurants}
            selectedRestaurantId={selectedRestaurantId}
            isDeleting={isDeleting}
            onSelectRestaurant={setSelectedRestaurantId}
            onAddRestaurant={() => setShowAddRestaurantDialog(true)}
            onDeleteRestaurant={handleDeleteRestaurant}
          />
        </Box>

        {/* Menu Items */}
        <Box sx={{ flex: { md: 0.6 } }}>
          <MenuItemsList
            selectedRestaurant={selectedRestaurant}
            isDeletingMeal={isDeletingMeal}
            onAddMeal={() => setShowAddMealDialog(true)}
            onDeleteMeal={handleDeleteMeal}
          />
        </Box>
      </Box>

      {/* Add Restaurant Dialog */}
      <AddRestaurantDialog
        open={showAddRestaurantDialog}
        isRegistering={isRegistering}
        restaurantForm={restaurantForm}
        onClose={() => setShowAddRestaurantDialog(false)}
        onSubmit={handleAddRestaurant}
        onFormChange={setRestaurantForm}
      />

      {/* Add Meal Dialog */}
      <AddMealDialog
        open={showAddMealDialog}
        isCreatingMeal={isCreatingMeal}
        mealForm={mealForm}
        modifiers={currentModifiers}
        onClose={() => {
          setShowAddMealDialog(false);
          setCurrentModifiers([]);
        }}
        onSubmit={handleAddMeal}
        onMealFormChange={setMealForm}
        onModifiersChange={setCurrentModifiers}
      />
    </Container>
  );
};

export default RestaurantsPage;
