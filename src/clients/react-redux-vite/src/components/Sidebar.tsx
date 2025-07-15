import { useIsAuthenticated, useMsal } from '@azure/msal-react';
import { Drawer, Toolbar, List, ListItemText, ListSubheader, ListItemButton } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import { useAppDispatch } from '../app/hooks';
import { showNotification } from '../features/notifications/notificationSlice';
import { setCredentials } from '../app/authSlice';

const drawerWidth = 240;

const Sidebar = () => {
  const { instance, accounts } = useMsal();
  const isAuthenticated = useIsAuthenticated();
  const dispatch = useAppDispatch()
  
  const handleLoginPopup = () => {
    instance.loginPopup()
      .then((tokenResponse) => {
        dispatch(setCredentials({
          accessToken: tokenResponse.accessToken,
          user: tokenResponse.account,
        }));
        
        dispatch(showNotification({ message: 'Login successful', severity: 'success' } ));
      })
      .catch((error) => console.error(error));
  };

  const handleLogoutPopup = () => {
    instance.logoutPopup()
      .catch((error) => console.error(error));
  };

  return (
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
      open
    >
      <div>
        {isAuthenticated ? (
          <>
            <p>Welcome, {accounts[0]?.name}!</p>
            <button onClick={handleLogoutPopup}>Logout</button>
          </>
        ) : (
          <>
            <p>Please sign in.</p>
            <button onClick={handleLoginPopup}>Login</button>
          </>
        )}
      </div>

      <Toolbar />

      <List subheader={<ListSubheader>General</ListSubheader>}>
        <ListItemButton component={RouterLink} to="/">
          <ListItemText primary="Home" />
        </ListItemButton>
        <ListItemButton component={RouterLink} to="/quotes">
          <ListItemText primary="Quotes" />
        </ListItemButton>
        <ListItemButton component={RouterLink} to="/theme">
          <ListItemText primary="Theme" />
        </ListItemButton>
      </List>

      <List subheader={<ListSubheader>Admin</ListSubheader>}>
        <ListItemButton component={RouterLink} to="/restaurants">
          <ListItemText primary="Restaurants" />
        </ListItemButton>
      </List>
    </Drawer>
  );
};

export default Sidebar;