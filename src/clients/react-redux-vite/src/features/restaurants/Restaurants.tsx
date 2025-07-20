import { JSX, useState } from "react"
import { Box, Button, Typography } from "@mui/material"
import { MasterDetailLayout } from "../../layouts/MasterDetailLayout"
import { RestaurantDetail } from "./components/detail"
import { RestaurantList } from "./components/list"
import { Add } from "@mui/icons-material"
import { RegisterRestaurantDialog } from "./components/register-dialog"

export const Restaurants = (): JSX.Element | null => {
  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [isDialogOpen, setIsDialogOpen] = useState(false);
  
  const handleSelectionChange = (id: string | null) => {
    setSelectedId(id);
  };

  const handleDelete = () => {
    setSelectedId(null);
  };

  const handleOpenDialog = () => {
    setIsDialogOpen(true);
  };

  const handleCloseDialog = () => {
    setIsDialogOpen(false);
  };

  const handleRegistrationSuccess = (restaurantId: string) => {
    setSelectedId(restaurantId);
  };

  return (
    <Box sx={{ p: 2 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4">
          Restaurants
        </Typography>
        <Button
          variant="contained"
          startIcon={<Add />}
          onClick={handleOpenDialog}
          size="large"
        >
          Register New
        </Button>
      </Box>
      
      <MasterDetailLayout
        master={
          <RestaurantList 
            onSelected={handleSelectionChange}
            selectedId={selectedId}
          />
        }
        detail={
          <RestaurantDetail 
            id={selectedId} 
            onDeleted={handleDelete} 
          />
        }
      />

      <RegisterRestaurantDialog
        open={isDialogOpen}
        onClose={handleCloseDialog}
        onSuccess={handleRegistrationSuccess}
      />
    </Box>
  );
}
