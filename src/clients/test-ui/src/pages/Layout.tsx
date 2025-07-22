import React from 'react';
import { AppBar, Toolbar, Typography, IconButton, Drawer, List, ListItem, ListItemText, Box } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { Link, Outlet } from 'react-router-dom';

const drawerWidth = 240;

const Layout: React.FC = () => {
  return (
    <Box sx={{ display: 'flex' }}>
      {/* AppBar */}
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Project: Elkhorn
          </Typography>
          <IconButton color="inherit">
            <AccountCircleIcon />
          </IconButton>
        </Toolbar>
      </AppBar>

      {/* Drawer */}
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          [`& .MuiDrawer-paper`]: { width: drawerWidth, boxSizing: 'border-box' }
        }}
      >
        <Toolbar />
        <List>
          {[
            { label: 'Restaurants', path: '/restaurants' },
            { label: 'Schools', path: '/schools' },
            { label: 'Settings', path: '/settings' }
          ].map((item) => (
            <ListItem key={item.label} component={Link} to={item.path}>
              <ListItemText primary={item.label} />
            </ListItem>
          ))}
        </List>
      </Drawer>

      {/* Content */}
      <Box component="main" sx={{ flexGrow: 1, p: 3, mt: 8 }}>
        <Outlet />
      </Box>
    </Box>
  );
};

export default Layout;