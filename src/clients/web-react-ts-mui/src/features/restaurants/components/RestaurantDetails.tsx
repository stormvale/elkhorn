import React from 'react';
import { Box, Typography, Button, Divider, List, ListItem } from '@mui/material';
import MenuItemCard from './MenuItemCard';

interface Props {
  restaurant: any;
  onAddMeal: () => void;
  onRemoveMeal: (id: string) => void;
}

const RestaurantDetails: React.FC<Props> = ({ restaurant, onAddMeal, onRemoveMeal }) => {
  if (!restaurant) return <Typography variant="body1">Select a restaurant</Typography>;

  const { name, contact, address, menu } = restaurant;

  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h5">{name}</Typography>
      <Typography variant="subtitle1">Contact: {contact.name} ({contact.type})</Typography>
      <Typography variant="body2">Email: {contact.email}</Typography>
      <Typography variant="body2">
        Address: {address.street}, {address.city}, {address.state} {address.postCode}
      </Typography>

      <Divider sx={{ my: 2 }} />
      <Typography variant="h6">Menu</Typography>
      <Button variant="outlined" onClick={onAddMeal}>Add Meal</Button>
      <List>
        {menu.map((item: any) => (
          <ListItem key={item.id}>
            <MenuItemCard item={item} onRemove={() => onRemoveMeal(item.id)} />
          </ListItem>
        ))}
      </List>
    </Box>
  );
};

export default RestaurantDetails;