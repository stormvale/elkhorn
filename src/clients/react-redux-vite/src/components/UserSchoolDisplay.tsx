import React from 'react';
import { Card, CardContent, Typography, Box, Chip } from '@mui/material';
import { useAuthenticatedUser, useSchoolContext } from '../hooks/useApp';

/**
 * Example component showing how to use the global user and school context
 * This can be used anywhere in your app after the AppContextProvider is set up
 */
const UserSchoolDisplay: React.FC = () => {
  const { user, isAuthenticated } = useAuthenticatedUser();
  const { currentSchool, availableSchools } = useSchoolContext();

  if (!isAuthenticated || !user) {
    return (
      <Card>
        <CardContent>
          <Typography>User not authenticated</Typography>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardContent>
        <Typography variant="h6" gutterBottom>
          Global Context Example
        </Typography>
        
        <Box sx={{ mb: 2 }}>
          <Typography variant="subtitle2" color="text.secondary">
            Current User:
          </Typography>
          <Typography>{user.name} ({user.email})</Typography>
          {user.roles.length > 0 && (
            <Box sx={{ mt: 1 }}>
              {user.roles.map((role: string) => (
                <Chip 
                  key={role} 
                  label={role} 
                  size="small" 
                  sx={{ mr: 1 }} 
                />
              ))}
            </Box>
          )}
        </Box>

        <Box sx={{ mb: 2 }}>
          <Typography variant="subtitle2" color="text.secondary">
            Current School:
          </Typography>
          {currentSchool ? (
            <Box>
              <Typography>{currentSchool.name}</Typography>
              {currentSchool.children && currentSchool.children.length > 0 && (
                <Typography variant="body2" color="text.secondary">
                  Children: {currentSchool.children.map((c: any) => c.name).join(', ')}
                </Typography>
              )}
            </Box>
          ) : (
            <Typography color="warning.main">No school selected</Typography>
          )}
        </Box>

        <Box>
          <Typography variant="subtitle2" color="text.secondary">
            Available Schools:
          </Typography>
          <Typography variant="body2">
            {availableSchools.length} school(s) available
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

export default UserSchoolDisplay;
