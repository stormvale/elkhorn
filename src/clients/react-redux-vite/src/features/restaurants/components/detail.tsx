import { Box, Card, CardContent, Divider, List, ListItem, Typography } from '@mui/material';
import { useGetRestaurantByIdQuery } from '../restaurantsApiSlice';
import MenuItemCard from './menu-item-card';

export const RestaurantDetail = ({ id }: { id: string | null }) => {
  const { data } = useGetRestaurantByIdQuery(id!, { skip: !id });

  if (!data) return <Typography variant="body2">Select a restaurant</Typography>;

  const { name, contact, address, menu } = data;

  return (
    <Box sx={{ p: 2 }}>
      <Card>
        <CardContent>
          <Typography variant="h4">{name}</Typography>
          <Typography variant="body2">Contact: {contact.name} ({contact.type})</Typography>
          <Typography variant="body2">Email: {contact.email}</Typography>
          <Typography variant="body2">Address: {address.street}, {address.city}, {address.state} {address.postCode}</Typography>
        </CardContent>
      </Card>

      <Divider sx={{ my: 2 }} />
      
      <Typography variant="h6">Menu</Typography>
      <List>
        {menu.map((item: any) => (
          <ListItem key={item.id}>
            <MenuItemCard item={item} onRemove={() => console.log('removed meal') } />
          </ListItem>
        ))}
      </List>

    </Box>
  );
};