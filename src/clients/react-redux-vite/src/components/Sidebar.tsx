import { Drawer, Toolbar, List, ListItemText, ListSubheader, ListItemButton } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

const drawerWidth = 240;

const Sidebar = () => {
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
      <Toolbar />

      <List subheader={<ListSubheader>General</ListSubheader>}>
        <ListItemButton component={RouterLink} to="/home">
          <ListItemText primary="Home" />
        </ListItemButton>
        <ListItemButton component={RouterLink} to="/counter">
          <ListItemText primary="Counter" />
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