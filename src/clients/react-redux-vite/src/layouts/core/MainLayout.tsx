import React from 'react';
import { Box, Drawer, Toolbar } from '@mui/material';
import Header from './Header';
import Sidebar from './SideBar';

const drawerWidth = 240;

interface LayoutProps {
  children: React.ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <Box sx={{ display: 'flex' }}>
      {/* Header */}
      <Header />
      
      {/* Sidebar */}
      <Drawer
        variant="permanent"
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar /> {/* This creates space for the header */}
        <Sidebar />
      </Drawer>
      
      {/* Main Content */}
      <Box
        component="main"
        sx={{
          flexGrow: 1,
          bgcolor: 'background.default',
          p: 3,
        }}
      >
        <Toolbar /> {/* This creates space for the header */}
        {children}
      </Box>
    </Box>
  );
};

export default Layout;