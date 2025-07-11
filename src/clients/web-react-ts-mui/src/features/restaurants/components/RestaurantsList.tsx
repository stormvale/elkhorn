import React from 'react';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Box } from '@mui/material';
import { Restaurant } from '../restaurantsSlice';

interface Props {
  restaurants: Restaurant[];
  onSelect: (restaurant: Restaurant) => void;
}

const columns: GridColDef[] = [
  { field: 'name', headerName: 'Name', flex: 1 },
  { field: 'contactName', headerName: 'Contact', flex: 1, valueGetter: (_value, row: Restaurant) => row.contact.name },
  { field: 'city', headerName: 'City', flex: 1, valueGetter: (_value, row: Restaurant) => row.address.city },
];

const RestaurantList: React.FC<Props> = ({ restaurants, onSelect }) => {
  return (
    <Box sx={{ height: 600 }}>
      <DataGrid
        rows={restaurants}
        columns={columns}
        getRowId={(r) => r.id}
        onRowClick={(params) => onSelect(params.row)}
      />
    </Box>
  );
};

export default RestaurantList;