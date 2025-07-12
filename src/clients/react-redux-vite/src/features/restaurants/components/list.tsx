import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Restaurant, useGetRestaurantsQuery } from '../restaurantsApiSlice';
import { Box } from '@mui/material';

const columns: GridColDef[] = [
  { field: 'id', headerName: 'ID', width: 200 },
  { field: 'name', headerName: 'Name', flex: 1 },
  { field: 'contact', headerName: 'Contact', flex: 2, valueGetter: (_value, row: Restaurant) => row.contact.name },
  { field: 'city', headerName: 'City', flex: 1, valueGetter: (_value, row: Restaurant) => row.address.city },
];

export const RestaurantList = ({ onSelect }: { onSelect: (id: string) => void }) => {
  const { data = [], isLoading } = useGetRestaurantsQuery();
  return (
    <Box sx={{ height: 400 }}>
      <DataGrid
        rows={data}
        columns={columns}
        getRowId={(r) => r.id}
        onRowClick={(params) => onSelect(params.row.id)}
        loading={isLoading}
      />
    </Box>
  );
};