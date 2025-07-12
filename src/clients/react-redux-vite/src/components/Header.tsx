import { AppBar, Toolbar, Typography, IconButton } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { ThemeToggle } from '../features/theme/ThemeToggle';

interface HeaderProps {
  onMenuClick: () => void;
}

export const Header = ({ onMenuClick }: HeaderProps) => (
  <AppBar position="fixed">
    <Toolbar>
      <IconButton color="inherit" edge="start" onClick={onMenuClick} sx={{ mr: 2 }}>
        <MenuIcon />
      </IconButton>
      <Typography variant="h6" sx={{ flexGrow: 1 }}>
        Project: Elkhorn
      </Typography>
      <ThemeToggle />
    </Toolbar>
  </AppBar>
);