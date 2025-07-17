import { Box, Grid } from '@mui/material';

interface MasterDetailLayoutProps {
  master: React.ReactNode;
  detail: React.ReactNode;
}

export const MasterDetailLayout = ({ master, detail }: MasterDetailLayoutProps) => (
  <Box sx={{ flexGrow: 1, p: 2 }}>
    <Grid container spacing={2}>
      <Grid size={3}>
        {master}
      </Grid>
      <Grid size={9}>
        {detail}
      </Grid>
    </Grid>
  </Box>
);