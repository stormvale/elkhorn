import { Typography, Stack, Container, Card, CardContent, Button, Box, Chip, Avatar } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuthenticatedUser, useSchoolContext, useLogout } from '../../hooks/useApp';
import SchoolChip from '../../components/SchoolChip';

const Home = () => {
  const navigate = useNavigate();
  const { user, isAuthenticated } = useAuthenticatedUser();
  const { currentSchool } = useSchoolContext();
  const { logout } = useLogout();

  // If not authenticated, redirect to auth landing
  if (!isAuthenticated) {
    navigate('/');
    return null;
  }

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  const getUserRoleDisplay = () => {
    if (user?.roles && user.roles.length > 0) {
      return user.roles.join(', ');
    }
    return 'no roles assigned';
  };

  if (!user) {
    return (
      <Container sx={{ py: 2, textAlign: 'center' }}>
        <Typography variant="h4">Loading user information...</Typography>
      </Container>
    );
  }

  return (
    <Container sx={{ py: 2, position: 'relative' }}>
      {/* Header Section */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Welcome to Project: Elkhorn
        </Typography>
        <Button 
          variant="outlined" 
          color="secondary" 
          onClick={handleLogout}
          sx={{ height: 'fit-content' }}
        >
          Logout
        </Button>
      </Box>

      {/* Main Content */}
      <Stack gap={2} my={2}>
        <Typography textAlign="center" variant="h5">
          School Communication Hub
        </Typography>
        <Typography textAlign="center" variant="body1" color="text.secondary">
          Stay connected with your child's education
        </Typography>
        
        {/* Placeholder for future features */}
        <Box sx={{ mt: 4, p: 3, bgcolor: 'grey.50', borderRadius: 2 }}>
          <Typography variant="h6" gutterBottom>
            Coming Soon:
          </Typography>
          <ul>
            <li>Messages from teachers</li>
            <li>School announcements</li>
            <li>Hot Lunch orders</li>
            <li>Lost-property tracking</li>
            <li>Event notifications</li>
            <li>Grade updates</li>
            <li>Field-trips</li>
            <li>Parent-teacher conference scheduling</li>
          </ul>
        </Box>
      </Stack>
    </Container>
  );
};

export default Home;
