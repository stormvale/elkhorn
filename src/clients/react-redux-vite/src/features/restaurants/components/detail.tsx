import { useState, useEffect } from 'react';
import { Box, Card, CardContent, Divider, IconButton, Typography, Alert } from '@mui/material';
import { 
  useDeleteRestaurantMutation, 
  useGetRestaurantByIdQuery,
  RestaurantResponse,
  RestaurantMealResponse 
} from '../api/apiSlice-generated';
import { Delete, Save } from '@mui/icons-material';
import { useAppDispatch } from '../../../app/hooks';
import { showNotification } from '../../notifications/notificationSlice';
import MenuItemCard from './menu-item-card';

interface RestaurantDetailProps {
  id: string | null;
  onDeleted: () => void;
}

export const RestaurantDetail = ({ id, onDeleted }: RestaurantDetailProps) => {
  const dispatch = useAppDispatch();
  const [localRestaurant, setLocalRestaurant] = useState<RestaurantResponse | null>(null);
  const [hasChanges, setHasChanges] = useState(false);

  const { data, isLoading, error } = useGetRestaurantByIdQuery(id!, { skip: !id });
  const [deleteRestaurant, { isLoading: isDeleting }] = useDeleteRestaurantMutation();
  
  // Note: You'll need to add an update mutation to your API if it doesn't exist
  // const [updateRestaurant, { isLoading: isSaving }] = useUpdateRestaurantMutation();

  // Update local state when data changes
  useEffect(() => {
    if (data) {
      setLocalRestaurant(data);
      setHasChanges(false);
    }
  }, [data]);

  const handleDeleteClick = async () => {
    if (window.confirm(`Are you sure you want to delete ${data?.name}?`)) {
      try {
        onDeleted();
        await deleteRestaurant(id!).unwrap();
        dispatch(showNotification({ 
          message: 'Restaurant was deleted', 
          severity: 'success' 
        }));
      } catch (error) {
        console.error('Delete failed:', error);
        dispatch(showNotification({ 
          message: 'Failed to delete restaurant', 
          severity: 'error' 
        }));
      }
    }
  };

  const handleMenuItemDelete = async (menuItemId: string) => {
    if (!localRestaurant) return;

    // Use the existing API endpoint for deleting meals
    try {
      // You can call the delete meal mutation here if you want immediate deletion
      // await deleteRestaurantMeal({ restaurantId: localRestaurant.id, mealId: menuItemId }).unwrap();
      
      // Or just update local state for batch saving
      const updatedMenu = localRestaurant.menu.filter((item) => item.id !== menuItemId);
      setLocalRestaurant({ ...localRestaurant, menu: updatedMenu });
      setHasChanges(true);
    } catch (error) {
      dispatch(showNotification({ 
        message: 'Failed to delete menu item', 
        severity: 'error' 
      }));
    }
  };

  const handleModifierRemove = (menuItemId: string, modifierName: string) => {
    if (!localRestaurant) return;

    const updatedMenu = localRestaurant.menu.map((item: RestaurantMealResponse) => {
      if (item.id === menuItemId) {
        return {
          ...item,
          availableModifiers: item.availableModifiers.filter(
            (modifier) => modifier.name !== modifierName
          )
        };
      }
      return item;
    });

    setLocalRestaurant({ ...localRestaurant, menu: updatedMenu });
    setHasChanges(true);
  };

  const handleSave = async () => {
    dispatch(showNotification({ 
      message: 'Save not yet supported by Restaurants API', 
      severity: 'info' 
    }));
    
    // update endpoint would work something like this:
    // try {
    //   await updateRestaurant({
    //     id: localRestaurant.id,
    //     ...localRestaurant
    //   }).unwrap();
    //   setHasChanges(false);
    //   dispatch(showNotification({ 
    //     message: 'Restaurant updated successfully', 
    //     severity: 'success' 
    //   }));
    // } catch (error) {
    //   dispatch(showNotification({ 
    //     message: 'Failed to save restaurant', 
    //     severity: 'error' 
    //   }));
    // }
  };

  // If nothing is selected, show a prompt
  if (!id) {
    return (
      <Box sx={{ p: 2, textAlign: 'center' }}>
        <Typography variant="body1" color="text.secondary">
          Select a restaurant to view details
        </Typography>
      </Box>
    );
  }

  // Loading state
  if (isLoading) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography variant="body1">Loading restaurant details...</Typography>
      </Box>
    );
  }

  // Error state
  if (error || !data || !localRestaurant) {
    return (
      <Alert severity="error" sx={{ mb: 2 }}>
        {error ? 'Failed to load restaurant details' : 'Restaurant not found'}
      </Alert>
    );
  }

  const { name, contact, address, menu } = localRestaurant;

  return (
    <Box>
      
      <Card sx={{ position: 'relative' }}>
        <Box sx={{ position: 'absolute', top: 8, right: 8, display: 'flex', gap: 1 }}>
          {hasChanges && (
            <IconButton
              onClick={handleSave}
              color="primary"
              aria-label="save changes"
              //disabled={isSaving}
              sx={{ ml: 1 }}
            >
              <Save />
            </IconButton>
          )}
          <IconButton 
            onClick={handleDeleteClick} 
            color="error" 
            aria-label="delete restaurant"
            disabled={isDeleting}
          >
            <Delete />
          </IconButton>
        </Box>
        
        <CardContent sx={{ pr: 6 }}>
          <Typography variant="h4" gutterBottom>{name}</Typography>
          
          <Typography variant="body1">
            Contact: {contact.name} ({contact.type})
          </Typography>
          <Typography variant="body1">
            Email: {contact.email}
          </Typography>
          {contact.phone && (
            <Typography variant="body1">
              Phone: {contact.phone}
            </Typography>
          )}
          
          <Typography variant="body1">
            Address: {address.street}, {address.city}, {address.state} {address.postCode}
          </Typography>
        </CardContent>
      </Card>

      <Divider sx={{ my: 2 }} />
      
      <Typography variant="h6" gutterBottom>Menu</Typography>
      
      {menu && menu.length > 0 ? (
        <Box 
          sx={{ 
            display: 'grid', 
            gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' },
            gap: 2 
          }}
        >
          {menu.map((item: any) => (
            <MenuItemCard 
              item={item} 
              onDelete={handleMenuItemDelete}
              onModifierRemove={handleModifierRemove}
            />
          ))}
        </Box>
      ) : (
        <Typography variant="body2" color="text.secondary">
          No menu items available
        </Typography>
      )}
    </Box>
  );
};