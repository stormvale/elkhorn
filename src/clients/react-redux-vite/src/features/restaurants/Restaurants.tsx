import { JSX, useState } from "react"
import { Box, Typography } from "@mui/material"
import { MasterDetailLayout } from "../../components/MasterDetailLayout"
import { RestaurantDetail } from "./components/detail"
import { RestaurantList } from "./components/list"

export const Restaurants = (): JSX.Element | null => {
  const [selectedId, setSelectedId] = useState<string | null>(null);

  return (
    <Box sx={{ display: 'flex', gap: 4, p: 2 }}>
      <Box sx={{ flex: 1 }}>
        <Typography variant="h4" gutterBottom>Restaurants</Typography>
          <MasterDetailLayout
            master={<RestaurantList onSelect={setSelectedId} />}
            detail={<RestaurantDetail id={selectedId} />}
          />
      </Box>
    </Box>
  );
}
