import React from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Avatar,
  Menu,
  MenuItem,
  Chip
} from '@mui/material';
import { ThemeToggle } from '../../theme/ThemeToggle';
import { useAuthenticatedUser, useSchoolContext, useLogout } from '../../hooks/useApp';

const Header: React.FC = () => {
  const { user } = useAuthenticatedUser();
  const { currentSchool } = useSchoolContext();
  const { logout } = useLogout();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error('Logout error:', error);
    }
    handleClose();
  };

  return (
    <AppBar 
      position="fixed" 
      sx={{ 
        zIndex: (theme) => theme.zIndex.drawer + 1,
        bgcolor: 'primary.main'
      }}
    >
      <Toolbar>
        <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
          Project: Elkhorn
        </Typography>
        
        {/* Show current school context */}
        {currentSchool && (
          <Chip 
            label={currentSchool.name}
            size="small"
            variant="outlined"
            sx={{ 
              bgcolor: 'rgba(255, 255, 255, 0.1)',
              color: 'white',
              borderColor: 'rgba(255, 255, 255, 0.3)'
            }}
          />
        )}
        
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <ThemeToggle />
          
          {user && (
            <>
              <Typography variant="body2">
                Welcome, {user.name}
              </Typography>
              <Avatar
                onClick={handleMenu}
                sx={{ cursor: 'pointer', bgcolor: 'secondary.main' }}
              >
                {user.name?.charAt(0).toUpperCase()}
              </Avatar>
              <Menu
                anchorEl={anchorEl}
                open={Boolean(anchorEl)}
                onClose={handleClose}
              >
                <MenuItem onClick={handleClose}>Profile</MenuItem>
                <MenuItem onClick={handleLogout}>Logout</MenuItem>
              </Menu>
            </>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;