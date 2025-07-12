import { useDispatch } from 'react-redux';
import { IconButton } from '@mui/material';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import { toggleDarkMode } from './themeSlice';

export const ThemeToggle = () => {
  const dispatch = useDispatch();

  return (
    <IconButton onClick={() => dispatch(toggleDarkMode())} color="inherit">
      <Brightness4Icon />
    </IconButton>
  );
};