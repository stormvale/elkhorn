import { Drawer, Toolbar, List, ListItemText, ListSubheader, ListItemButton } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

interface SidebarProps {
  open: boolean;
  onClose: () => void;
  pinned?: boolean;
  drawerWidth?: number;
}

export const Sidebar = ({
  open,
  onClose,
  pinned = false,
  drawerWidth = 240,
}: SidebarProps) => (
  <Drawer
    variant={pinned ? 'persistent' : 'temporary'}
    open={open}
    onClose={onClose}
    sx={{ '& .MuiDrawer-paper': { width: drawerWidth } }}
  >
    <Toolbar />

    <List subheader={<ListSubheader>General</ListSubheader>}>
      <ListItemButton component={RouterLink} to="/">
        <ListItemText primary="Home" />
      </ListItemButton>
      <ListItemButton component={RouterLink} to="/quotes">
        <ListItemText primary="Quotes" />
      </ListItemButton>
      <ListItemButton component={RouterLink} to="/about">
        <ListItemText primary="About" />
      </ListItemButton>
    </List>

    <List subheader={<ListSubheader>Admin</ListSubheader>}>
      <ListItemButton component={RouterLink} to="/restaurants">
        <ListItemText primary="Restaurants" />
      </ListItemButton>
    </List>
  </Drawer>
);