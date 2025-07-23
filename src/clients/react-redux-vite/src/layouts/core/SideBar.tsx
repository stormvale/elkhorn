import React from 'react';
import {
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Divider,
  Box,
  Typography,
  Stack,
} from '@mui/material';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useAuthenticatedUser, useSchoolContext } from '../../hooks/useApp';
import sidebarRoutes from '../../routes/routes';
import SchoolChip from '../../components/SchoolChip';

const Sidebar: React.FC = () => {
  const location = useLocation();
  const { user } = useAuthenticatedUser();
  const { currentSchool } = useSchoolContext();

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
      <List sx={{ pt: 0 }}>
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
      <Box sx={{ 
        position: 'absolute', 
        bottom: 0, 
        width: '100%', 
        p: 2,
        bgcolor: 'grey.50',
        borderTop: 1,
        borderColor: 'divider'
      }}>
        {user && (
          <Box>
            <Stack spacing={1.5} sx={{ mb: 1 }}>
              <Box sx={{ flex: 1, minWidth: 0 }}>
                <Typography variant="body2" noWrap fontWeight="medium">
                  {user.name}
                </Typography>
                <Typography variant="caption" color="text.secondary" noWrap>
                  {user.roles.join(', ') || 'No roles assigned'}
                </Typography>
              </Box>
            </Stack>

            {/* School Context */}
            {currentSchool && (
              <Box sx={{ mb: 2 }}>
                <SchoolChip size="small" variant="outlined" />
              </Box>
            )}

            {/* Children List */}
            {currentSchool?.children && currentSchool.children.length > 0 && (
              <Box>
                <Stack spacing={0.5}>
                  {currentSchool.children.map((child: any) => (
                    <Typography variant="caption" color="text.secondary" noWrap>
                      {`${child.name} - ${child.grade}`}
                    </Typography>
                  ))}
                </Stack>
              </Box>
            )}
          </Box>
        )}
      </Box>
    </Box>
  );
};

export default Sidebar;