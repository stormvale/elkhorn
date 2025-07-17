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
import {
  Home as HomeIcon,
  Restaurant as RestaurantIcon,
  FormatQuote as QuoteIcon,
  Palette as ThemeIcon,
  Dashboard as DashboardIcon
} from '@mui/icons-material';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';
import routes from '../../routes/routes';

// Icon mapping for different routes
const getIconForPath = (path: string) => {
  switch (path) {
    case '/home':
      return <HomeIcon />;
    case '/restaurants':
      return <RestaurantIcon />;
    case '/quotes':
      return <QuoteIcon />;
    case '/theme':
      return <ThemeIcon />;
    default:
      return <DashboardIcon />;
  }
};

// Get display name for routes
const getDisplayName = (path: string) => {
  switch (path) {
    case '/home':
      return 'Home';
    case '/restaurants':
      return 'Restaurants';
    case '/quotes':
      return 'Quotes';
    case '/theme':
      return 'Theme';
    default:
      return path.replace('/', '').charAt(0).toUpperCase() + path.slice(2);
  }
};

const Sidebar: React.FC = () => {
  const location = useLocation();
  const { user } = useSelector((state: RootState) => state.auth);

  // Filter routes that require auth and check user roles
  const availableRoutes = routes.filter(route => {
    // Skip login page and routes that don't require auth
    if (!route.requiresAuth || route.path === '/login' || route.path === '/') {
      return false;
    }

    // Check if user has required roles
    if (route.allowedRoles.length > 0 && user) {
      return user.roles.some(userRole => route.allowedRoles.includes(userRole));
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
                {getIconForPath(route.path)}
              </ListItemIcon>
              <ListItemText primary={getDisplayName(route.path)} />
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