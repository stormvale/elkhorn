import React from 'react';
import { Box } from '@mui/material';
import { DataGrid, GridColDef, GridActionsCellItem } from '@mui/x-data-grid';
import { Edit as EditIcon, Delete as DeleteIcon } from '@mui/icons-material';
import { UserSchool } from '../../../app/authSlice';
import { LunchResponse } from '../api/apiSlice-generated';

interface Restaurant {
  id: string;
  name: string;
}

interface LunchesGridProps {
  lunches: LunchResponse[];
  restaurants: Restaurant[];
  currentSchool: UserSchool | null;
  isDeleting: boolean;
  onEditLunch: (lunch: LunchResponse) => void;
  onDeleteLunch: (lunchId: string) => void;
}

const LunchesGrid: React.FC<LunchesGridProps> = ({
  lunches,
  restaurants,
  currentSchool,
  isDeleting,
  onEditLunch,
  onDeleteLunch
}) => {
  // Get restaurant name helper
  const getRestaurantName = (restaurantId: string) => {
    const restaurant = restaurants.find(r => r.id === restaurantId);
    return restaurant?.name || 'Unknown Restaurant';
  };

  // DataGrid columns
  const columns: GridColDef[] = [
    {
      field: 'date',
      headerName: 'Date',
      flex: 1,
      minWidth: 120,
      valueFormatter: (value) => {
        return new Date(value).toLocaleDateString();
      }
    },
    {
      field: 'restaurantId',
      headerName: 'Restaurant',
      flex: 2,
      minWidth: 200,
      valueGetter: (value) => getRestaurantName(value)
    },
    {
      field: 'schoolId',
      headerName: 'School',
      flex: 2,
      minWidth: 200,
      valueGetter: () => currentSchool?.schoolName || 'Current School'
    },
    {
      field: 'restaurantLunchItems',
      headerName: 'Items',
      flex: 1,
      minWidth: 100,
      valueGetter: (value: any[]) => value?.length || 0,
      renderCell: (params) => `${params.value} items`
    },
    {
      field: 'actions',
      type: 'actions',
      headerName: 'Actions',
      width: 120,
      sortable: false,
      filterable: false,
      disableColumnMenu: true,
      getActions: (params) => [
        <GridActionsCellItem
          key="edit"
          icon={<EditIcon />}
          label="Edit"
          onClick={() => onEditLunch(params.row)}
          disabled={isDeleting}
        />,
        <GridActionsCellItem
          key="delete"
          icon={<DeleteIcon />}
          label="Delete"
          onClick={() => onDeleteLunch(params.row.lunchId)}
          disabled={isDeleting}
        />
      ]
    }
  ];

  return (
    <Box sx={{ height: 600, width: '100%' }}>
      <DataGrid
        rows={lunches}
        columns={columns}
        getRowId={(row) => row.lunchId}
        pagination
        pageSizeOptions={[5, 10, 25]}
        initialState={{
          pagination: {
            paginationModel: { page: 0, pageSize: 10 }
          }
        }}
        disableRowSelectionOnClick
        loading={isDeleting}
      />
    </Box>
  );
};

export default LunchesGrid;
