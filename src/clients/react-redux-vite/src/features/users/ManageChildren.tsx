import React from 'react';
import { 
  Container, 
  Typography, 
  Paper, 
  Box,
  Button,
  Card,
  CardContent
} from '@mui/material';
import { Add as AddIcon, Person as PersonIcon } from '@mui/icons-material';
import { useAuthContext } from '../../hooks/useAuthContext';

const ManageChildren: React.FC = () => {
  const { currentUser } = useAuthContext();

  const handleAddChild = () => {
    // TODO: Implement add child functionality
    console.log('Add child clicked');
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Manage Children
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Add or update information about your children
        </Typography>
      </Box>

      <Box sx={{ display: 'flex', gap: 3, flexDirection: { xs: 'column', md: 'row' } }}>
        {/* Current Children */}
        <Box sx={{ flex: { md: 2 } }}>
          <Paper sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
              <Typography variant="h6">
                Your Children
              </Typography>
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={handleAddChild}
              >
                Add Child
              </Button>
            </Box>

            {currentUser?.children && currentUser.children.length > 0 ? (
              <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' }, gap: 2 }}>
                {currentUser.children.map((child, index) => (
                  <Card variant="outlined" key={child.childId || index}>
                    <CardContent>
                      <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
                        <PersonIcon color="primary" sx={{ mr: 1 }} />
                        <Typography variant="h6">
                          {child.firstName} {child.lastName}
                        </Typography>
                      </Box>
                      <Typography variant="body2" color="text.secondary">
                        Grade: {child.grade}
                      </Typography>
                      <Typography variant="body2" color="text.secondary">
                        School: {child.schoolName }
                      </Typography>
                    </CardContent>
                  </Card>
                ))}
              </Box>
            ) : (
              <Box sx={{ textAlign: 'center', py: 4 }}>
                <PersonIcon sx={{ fontSize: 48, color: 'text.secondary', mb: 2 }} />
                <Typography variant="body1" color="text.secondary">
                  No children registered yet
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Click "Add Child" to get started
                </Typography>
              </Box>
            )}
          </Paper>
        </Box>

        {/* Quick Actions */}
        <Box sx={{ flex: { md: 1 } }}>
          <Paper sx={{ p: 3 }}>
            <Typography variant="h6" gutterBottom>
              Quick Actions
            </Typography>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Button 
                variant="outlined" 
                fullWidth
                startIcon={<AddIcon />}
                onClick={handleAddChild}
              >
                Register New Child
              </Button>
              <Button 
                variant="outlined" 
                fullWidth
                disabled
              >
                Update Child Info
              </Button>
              <Button 
                variant="outlined" 
                fullWidth
                disabled
              >
                Transfer School
              </Button>
            </Box>
          </Paper>
        </Box>
      </Box>
    </Container>
  );
};

export default ManageChildren;
