import { useDispatch, useSelector } from 'react-redux';
import { IconButton } from '@mui/material';
import Brightness4Icon from '@mui/icons-material/Brightness4';
import { toggleDarkMode } from './themeSlice';
import { RootState } from '../../app/store';

export const ThemeToggle = () => {
  const dispatch = useDispatch();
  const mode = useSelector((state: RootState) => state.theme.mode);
  
  return (
    <IconButton onClick={() => dispatch(toggleDarkMode())} color="inherit">
      <Brightness4Icon
          sx={{
            transition: 'transform 0.4s',
            transform: mode === 'dark' ? 'rotateY(180deg)' : 'rotateY(0deg)',
          }}
        />
    </IconButton>
  );
};