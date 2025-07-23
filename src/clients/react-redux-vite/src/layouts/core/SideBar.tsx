import React from 'react';
import {
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Divider,
  Box,
  Typography
} from '@mui/material';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import sidebarRoutes from '../../routes/routes';

const Sidebar: React.FC = () => {
  const location = useLocation();
  const { user } = useSelector((state: RootState) => state.auth);

  // Filter routes that require auth and check user roles
  const availableRoutes = sidebarRoutes.filter(route => {
   
    // Check if user has required roles
    if (route.allowedRoles.length > 0 && user) {
      return user.roles.some((userRole: string) => route.allowedRoles.includes(userRole));
    }

    return true;
  });

  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <Box sx={{ p: 2 }}>
        <Typography variant="h6" color="text.secondary">
          Navigation
        </Typography>
      </Box>
      
      <Divider />
      
      <List>
        {availableRoutes.map((route) => (
          <ListItem key={route.path} disablePadding>
            <ListItemButton
              component={RouterLink}
              to={route.path}
              selected={location.pathname === route.path}
              sx={{
                '&.Mui-selected': {
                  backgroundColor: 'action.selected',
                },
              }}
            >
              <ListItemIcon>
                {route.icon}
              </ListItemIcon>
              <ListItemText primary={route.displayName} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>

      {/* User Info Section */}
      <Box sx={{ position: 'absolute', bottom: 0, width: '100%', p: 2 }}>
        <Divider sx={{ mb: 2 }} />
        {user && (
          <Box>
            <Typography variant="caption" color="text.secondary">
              Signed in as:
            </Typography>
            <Typography variant="body2" noWrap>
              {user.email}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              Roles: {user.roles.join(', ')}
            </Typography>
          </Box>
        )}
      </Box>
    </Box>
  );
};

export default Sidebar;