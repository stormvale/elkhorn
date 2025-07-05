import { Typography, Stack, Container } from '@mui/material';
import TemplateTester from '@/components/TemplateTester/TemplateTester';
import Counter from '@/components/Counter/Counter';
import RestaurantsList from "@/components/Restaurants/RestaurantsList.tsx";

const Home = () => {
  return (
    <Container sx={{ py: 2, position: 'relative' }}>
      <Stack gap={1} my={2}>
        <Typography textAlign="center" variant="h2">
          Project: Elkhorn
        </Typography>
        <Typography textAlign="center" variant="subtitle1">
          React + TS + Vite + Redux + RTK + MUI + RRD + Prettier
        </Typography>
      </Stack>
      <RestaurantsList />
      {/*<TemplateTester />*/}
      {/*<Counter />*/}
    </Container>
  );
};

export default Home;
