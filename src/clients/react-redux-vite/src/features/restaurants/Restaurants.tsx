import { JSX, useState } from "react"
import { Box, Typography } from "@mui/material"
import { MasterDetailLayout } from "../../layouts/MasterDetailLayout"
import { RestaurantDetail } from "./components/detail"
import { RestaurantList } from "./components/list"

export const Restaurants = (): JSX.Element | null => {
  const [selectedId, setSelectedId] = useState<string | null>(null);
  
  const handleSelectionChange = (id: string | null) => {
    setSelectedId(id);
  };

  const handleDelete = () => {
    // Clear selection when item is deleted
    setSelectedId(null);
  };

  return (
    <Box sx={{ p: 2 }}>
      <Typography variant="h4" gutterBottom>
        Restaurants
      </Typography>
      
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
    </Box>
  );
}
