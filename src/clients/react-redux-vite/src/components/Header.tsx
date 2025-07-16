import { Typography, Box, Stack } from '@mui/material';
import { ThemeToggle } from '../features/theme/ThemeToggle';
import { HeaderLogout } from '../features/auth/HeaderLogout';

const Header = () =>
  <Box
    sx={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      py: 2,
      px: 3,
      borderBottom: 1,
      borderColor: 'divider',
      bgcolor: 'background.paper',
    }}
  >
    <Typography variant="h6">Project: Elkhorn</Typography>
    <Stack direction="row" spacing={2} alignItems="center">
      <HeaderLogout />
      <ThemeToggle />
    </Stack>
  </Box>


export default Header;