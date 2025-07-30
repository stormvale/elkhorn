import React, { useState } from 'react';
import {
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Box,
  Typography,
  Stack,
  Collapse,
} from '@mui/material';
import { ExpandLess, ExpandMore } from '@mui/icons-material';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { useAuthContext } from '../../hooks/useAuthContext';
import sidebarRoutes, { sidebarSections } from '../../routes/routes';
import SchoolChip from '../../components/SchoolChip';

const Sidebar: React.FC = () => {
  const location = useLocation();
  const { currentUser, currentSchool } = useAuthContext();
  
  // State for section collapse/expand
  const [openSections, setOpenSections] = useState<Record<string, boolean>>(() => {
    const initialState: Record<string, boolean> = {};
    Object.keys(sidebarSections).forEach(sectionKey => {
      initialState[sectionKey] = sidebarSections[sectionKey as keyof typeof sidebarSections].defaultOpen;
    });
    return initialState;
  });

  // Filter routes that require auth and check user roles
  const availableRoutes = sidebarRoutes.filter(route => {
    // Check if user has required roles
    if (route.allowedRoles.length > 0 && currentUser) {
      return currentUser.roles.some((userRole: string) => route.allowedRoles.includes(userRole));
    }
    return true;
  });

  // Group routes by section
  const standaloneRoutes = availableRoutes.filter(route => !route.section);
  const sectionedRoutes = availableRoutes.filter(route => route.section);
  const routesBySection = sectionedRoutes.reduce((acc, route) => {
    if (!acc[route.section!]) {
      acc[route.section!] = [];
    }
    acc[route.section!].push(route);
    return acc;
  }, {} as Record<string, typeof availableRoutes>);

  const toggleSection = (sectionKey: string) => {
    setOpenSections(prev => ({
      ...prev,
      [sectionKey]: !prev[sectionKey]
    }));
  };

  return (
    <Box sx={{ width: '100%', height: '100%' }}>
      <List sx={{ pt: 0 }}>
        {/* Standalone routes (like Home) */}
        {standaloneRoutes.map((route) => (
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

        {/* Sectioned routes */}
        {Object.entries(routesBySection).map(([sectionKey, routes]) => {
          const sectionConfig = sidebarSections[sectionKey as keyof typeof sidebarSections];
          const isOpen = openSections[sectionKey];
          
          return (
            <React.Fragment key={sectionKey}>
              {/* Section Header */}
              <ListItem disablePadding>
                <ListItemButton onClick={() => toggleSection(sectionKey)}>
                  <ListItemIcon>
                    {sectionConfig.icon}
                  </ListItemIcon>
                  <ListItemText primary={sectionConfig.title} />
                  {isOpen ? <ExpandLess /> : <ExpandMore />}
                </ListItemButton>
              </ListItem>
              
              {/* Section Items */}
              <Collapse in={isOpen} timeout="auto" unmountOnExit>
                <List component="div" disablePadding>
                  {routes.map((route) => (
                    <ListItem key={route.path} disablePadding>
                      <ListItemButton
                        component={RouterLink}
                        to={route.path}
                        selected={location.pathname === route.path}
                        sx={{
                          pl: 4, // Indent nested items
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
              </Collapse>
            </React.Fragment>
          );
        })}
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
        {currentUser && (
          <Box>
            <Stack spacing={1.5} sx={{ mb: 1 }}>
              <Box sx={{ flex: 1, minWidth: 0 }}>
                <Typography variant="body2" noWrap fontWeight="medium">
                  {currentUser.name}
                </Typography>
                <Typography variant="caption" color="text.secondary" noWrap>
                  {currentUser.roles.join(', ') || 'No roles assigned'}
                </Typography>
              </Box>
            </Stack>

            {/* School Context */}
            {currentSchool && (
              <Box sx={{ mb: 2 }}>
                <SchoolChip size="small" variant="outlined" />
              </Box>
            )}

          </Box>
        )}
      </Box>
    </Box>
  );
};

export default Sidebar;