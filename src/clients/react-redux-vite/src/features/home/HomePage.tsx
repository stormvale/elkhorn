import { Typography, Stack, Container, Card, CardContent, Button, Box, Chip, Avatar } from '@mui/material';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { schoolContextService } from '../../services/schoolContextService';
import { UserSchoolDto } from '../users/api/apiSlice-generated';

const Home = () => {
  const { instance, accounts } = useMsal();
  const navigate = useNavigate();
  const [currentSchool, setCurrentSchool] = useState<UserSchoolDto | null>(null);
  const [userName, setUserName] = useState<string>('');
  const [userRoles, setUserRoles] = useState<string[]>([]);

  useEffect(() => {
    const initializeUserData = async () => {
      if (accounts.length > 0) {
        const account = accounts[0];
        setUserName(account.name || account.username || 'Unknown User');
        
        // Extract roles from the account's ID token claims
        const roles = account.idTokenClaims?.roles || account.idTokenClaims?.['extension_Roles'] || [];
        setUserRoles(Array.isArray(roles) ? roles : []);

        try {
          await schoolContextService.initialize();
          const school = schoolContextService.getCurrentSchool();
          setCurrentSchool(school);
        } catch (error) {
          console.error('Failed to initialize school context:', error);
        }
      } else {
        // No user logged in, redirect to auth landing
        navigate('/');
      }
    };

    initializeUserData();
  }, [accounts, navigate]);

  const handleLogout = async () => {
    try {
      // Clear the school context
      schoolContextService.clear();
      
      // Perform MSAL logout
      await instance.logoutRedirect({
        postLogoutRedirectUri: '/'
      });
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  const handleSwitchSchool = () => {
    navigate('/school-selector?mode=switch');
  };

  const getUserRoleDisplay = () => {
    if (userRoles.length > 0) {
      return userRoles.join(', ');
    }
    return 'Parent/Guardian'; // Fallback if no roles are defined
  };

  if (!accounts.length) {
    return (
      <Container sx={{ py: 2, textAlign: 'center' }}>
        <Typography variant="h4">Loading...</Typography>
      </Container>
    );
  }

  return (
    <Container sx={{ py: 2, position: 'relative' }}>
      {/* Header Section */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h4" component="h1">
          Welcome to Elkhorn
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

      {/* User Information Card */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
            <Avatar sx={{ bgcolor: 'primary.main' }}>
              {userName.charAt(0).toUpperCase()}
            </Avatar>
            <Box>
              <Typography variant="h6" component="h2">
                {userName}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {getUserRoleDisplay()}
              </Typography>
            </Box>
          </Stack>

          {currentSchool ? (
            <Box>
              <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                Current School Context:
              </Typography>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
                <Chip 
                  label={currentSchool.name}
                  color="primary"
                  variant="filled"
                />
                {schoolContextService.hasMultipleSchools() && (
                  <Button 
                    size="small" 
                    variant="text" 
                    onClick={handleSwitchSchool}
                  >
                    Switch School
                  </Button>
                )}
              </Box>
              
              {currentSchool.children && currentSchool.children.length > 0 && (
                <Box sx={{ mt: 2 }}>
                  <Typography variant="subtitle2" color="text.secondary" gutterBottom>
                    Your Children:
                  </Typography>
                  <Stack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                    {currentSchool.children.map((child) => (
                      <Chip 
                        key={child.id}
                        label={`${child.name} - ${child.grade}`}
                        variant="outlined"
                        size="small"
                      />
                    ))}
                  </Stack>
                </Box>
              )}
            </Box>
          ) : (
            <Box>
              <Typography variant="body2" color="warning.main">
                No school context selected. 
                <Button size="small" onClick={() => navigate('/school-selector')}>
                  Select a school
                </Button>
              </Typography>
            </Box>
          )}
        </CardContent>
      </Card>

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
