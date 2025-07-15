import { Paper, Stack, Typography } from '@mui/material';
import { useListRestaurantsQuery } from '../api/apiSlice';
import { RestaurantResponse } from '../api/apiSlice-generated';
import { useState } from 'react';

// const columns: GridColDef[] = [
//   { field: 'id', headerName: 'ID', width: 200 },
//   { field: 'name', headerName: 'Name', flex: 1 },
//   { field: 'contact', headerName: 'Contact', flex: 2, valueGetter: (_value, row: RestaurantResponse) => row.contact.name },
//   { field: 'city', headerName: 'City', flex: 1, valueGetter: (_value, row: RestaurantResponse) => row.address.city },
// ];

export const RestaurantList = ({ onSelect }: { onSelect: (id: string) => void }) => {
  const { data = [] } = useListRestaurantsQuery();
  const [selectedIndex, setSelectedIndex] = useState<number | null>(null);

  return (
    <Stack spacing={2}>
      {data.map((restaurant: RestaurantResponse, index) => (
        <Paper
          key={restaurant.id}
          elevation={1}
          sx={{
            padding: 2,
            cursor: 'pointer',
            backgroundColor: selectedIndex === index ? 'primary.light' : 'background.paper',
            borderColor: selectedIndex === index ? 'primary.main' : 'divider',
          }}
          onClick={() => {
            setSelectedIndex(index);
            onSelect(restaurant.id);
          }}
        >
          <Typography variant="h3">{restaurant.name}</Typography>
        </Paper>
      ))}
    </Stack>
  );
};