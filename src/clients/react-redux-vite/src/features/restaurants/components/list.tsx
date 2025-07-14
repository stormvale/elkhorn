import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Box } from '@mui/material';
import { useListRestaurantsQuery } from '../api/apiSlice';
import { RestaurantResponse } from '../api/apiSlice-generated';

const columns: GridColDef[] = [
  { field: 'id', headerName: 'ID', width: 200 },
  { field: 'name', headerName: 'Name', flex: 1 },
  { field: 'contact', headerName: 'Contact', flex: 2, valueGetter: (_value, row: RestaurantResponse) => row.contact.name },
  { field: 'city', headerName: 'City', flex: 1, valueGetter: (_value, row: RestaurantResponse) => row.address.city },
];

export const RestaurantList = ({ onSelect }: { onSelect: (id: string) => void }) => {
  const { data = [], isLoading } = useListRestaurantsQuery();
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