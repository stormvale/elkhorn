import { Box, Card, CardContent, Divider, IconButton, Stack, Typography } from '@mui/material';
import { useDeleteRestaurantMutation, useGetRestaurantByIdQuery } from '../api/apiSlice';
import { Delete } from '@mui/icons-material';
import MenuItemCard from './menu-item-card';

export const RestaurantDetail = ({ id }: { id: string | null }) => {
  const { data } = useGetRestaurantByIdQuery(id!, { skip: !id });
  const [deleteRestaurant] = useDeleteRestaurantMutation();

  if (!data) return <Typography variant="body1">Select a restaurant</Typography>;

  const { name, contact, address, menu } = data;
  return (
    <Box>
      <Card sx={{ position: 'relative', paddingRight: 5 }}>
        <IconButton onClick={() => deleteRestaurant(id!)} color="error" aria-label="delete" sx={{ position: 'absolute', top: 8, right: 8 }}>
          <Delete />
        </IconButton>
        <CardContent>
          <Typography variant="h4">{name}</Typography>
          <Typography variant="body1">Contact: {contact.name} ({contact.type})</Typography>
          <Typography variant="body1">Email: {contact.email}</Typography>
          <Typography variant="body1">Address: {address.street}, {address.city}, {address.state} {address.postCode}</Typography>
        </CardContent>
      </Card>

      <Divider sx={{ my: 2 }} />
      
      <Typography variant="h6">Menu</Typography>
      <Stack spacing={2}>
        {menu.map((item: any) => (
            <MenuItemCard item={item} onRemove={() => console.log('removed meal') } />
        ))}
      </Stack>

    </Box>
  );
};