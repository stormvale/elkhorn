import { Box, Grid } from '@mui/material';

interface MasterDetailLayoutProps {
  master: React.ReactNode;
  detail: React.ReactNode;
}

export const MasterDetailLayout = ({ master, detail }: MasterDetailLayoutProps) => (
  <Box sx={{ flexGrow: 1, p: 2 }}>
    <Grid container spacing={2}>
      <Grid size={7}>
        {master}
      </Grid>
      <Grid size={5}>
        {detail}
      </Grid>
    </Grid>
  </Box>
);