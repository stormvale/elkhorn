import { Alert, Box, Card, CardContent, Divider, IconButton, Stack, Typography } from '@mui/material';
import { useDeleteRestaurantMutation, useGetRestaurantByIdQuery } from '../api/apiSlice';
import { Delete } from '@mui/icons-material';
import MenuItemCard from './menu-item-card';
import { showNotification } from '../../notifications/notificationSlice';
import { useAppDispatch } from '../../../app/hooks';

interface RestaurantDetailProps {
  id: string | null;
  onDeleted: () => void;
}

export const RestaurantDetail = ({ id, onDeleted }: RestaurantDetailProps) => {
  const dispatch = useAppDispatch();

  const { data, isLoading, error } = useGetRestaurantByIdQuery(id!, { skip: !id });
  
  const [deleteRestaurant, { isLoading: isDeleting  }] = useDeleteRestaurantMutation();

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

  // loading...
  if (isLoading) {
    return (
      <Box sx={{ p: 2 }}>
        <Typography variant="body1">Loading restaurant details...</Typography>
      </Box>
    );
  }

  // error!
  if (error || !data) {
    return (
      <Alert severity="error" sx={{ mb: 2 }}>
        {error ? 'Failed to load restaurant details' : 'Restaurant not found'}
      </Alert>
    );
  }

  const { name, contact, address, menu } = data;

  const handleDeleteClick = async () => {
    try {
      onDeleted(); // clear the selection right before the delete to prevent refetch
      await deleteRestaurant(id!).unwrap();
      dispatch(showNotification({ message: 'Restaurant was deleted', severity: 'success' }));
    } catch (error) {
      dispatch(showNotification({ message: 'Failed to delete restaurant', severity: 'error' }));
    }
  };

  return (
    <Box>
      <Card sx={{ position: 'relative' }}>
        <IconButton
          onClick={handleDeleteClick}
          color="error"
          aria-label="delete restaurant"
          disabled={isDeleting}
          sx={{ position: 'absolute', top: 8, right: 8 }}
        >
          <Delete />
        </IconButton>

        <CardContent sx={{ pr: 6 }}>
          <Typography variant="h4" gutterBottom>{name}</Typography>
          
          {contact && (
            <>
              <Typography variant="body1">
                Contact: {contact.name} ({contact.type})
              </Typography>
              <Typography variant="body1">
                Email: {contact.email}
              </Typography>
            </>
          )}
          
          {address && (
            <Typography variant="body1">
              Address: {address.street}, {address.city}, {address.state} {address.postCode}
            </Typography>
          )}
        </CardContent>
      </Card>

      <Divider sx={{ my: 2 }} />
      
      <Typography variant="h6" gutterBottom>Menu</Typography>
      
      {menu && menu.length > 0 ? (
        <Stack spacing={2}>
          {menu.map((item: any) => (
            <MenuItemCard 
              key={item.id || item.name} 
              item={item} 
              onRemove={() => console.log('removed meal', item.id)} 
            />
          ))}
        </Stack>
      ) : (
        <Typography variant="body2" color="text.secondary">
          No menu items available
        </Typography>
      )}

    </Box>
  );
};