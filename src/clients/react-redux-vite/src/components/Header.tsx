import { Typography, Box } from '@mui/material';
import { ThemeToggle } from '../features/theme/ThemeToggle';

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
    <ThemeToggle />
  </Box>


export default Header;