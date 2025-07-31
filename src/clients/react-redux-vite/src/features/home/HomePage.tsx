import { Typography, Stack, Container, Box } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useAuthContext } from '../../hooks/useAuthContext';

const Home = () => {
  const navigate = useNavigate();
  const { currentUser, isAuthenticated } = useAuthContext();

  if (!isAuthenticated) {
    navigate('/');
    return null;
  }


  if (!currentUser) {
    return (
      <Container sx={{ py: 2, textAlign: 'center' }}>
        <Typography variant="h4">Loading user information...</Typography>
      </Container>
    );
  }

  return (
    <Container sx={{ py: 2, position: 'relative' }}>
      
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
            <li>District, School & PAC announcements</li>
            <li>Direct/class messages from teachers</li>
            <li>Parent-teacher conference scheduling</li>
            <li>Hot Lunch orders</li>
            <li>Lost-property tracking</li>
            <li>Event notifications</li>
            <li>Report cards</li>
            <li>Field-trips</li>
          </ul>
        </Box>
      </Stack>
    </Container>
  );
};

export default Home;
