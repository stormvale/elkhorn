import React from 'react';
import {
  AppBar,
  Toolbar,
  Typography,
  Box,
  Avatar,
  Menu,
  MenuItem
} from '@mui/material';
import { useMsal } from '@azure/msal-react';
import { useAppDispatch } from '../../app/hooks';
import { clearCredentials } from '../../app/authSlice';
import { ThemeToggle } from '../../theme/ThemeToggle';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';

const Header: React.FC = () => {
  const { instance } = useMsal();
  const { user } = useSelector((state: RootState) => state.auth);
  const dispatch = useAppDispatch();
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const handleMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleLogout = () => {
    instance.logoutPopup()
      .then(() => dispatch(clearCredentials()))
      .catch(error => console.error('Logout error:', error));

    handleClose()
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