import React from 'react';
import {
  Box,
  Button,
  Paper,
  Typography
} from '@mui/material';
import {
  Add as AddIcon,
  Person as PersonIcon
} from '@mui/icons-material';
import { UserChild } from '../../../app/authSlice';
import ChildCard from './ChildCard';

interface ChildrenListProps {
  children: UserChild[];
  onAddChild: () => void;
  onEditChild?: (child: UserChild) => void;
  onChildMenuClick?: (child: UserChild) => void;
}

const ChildrenList: React.FC<ChildrenListProps> = ({
  children,
  onAddChild,
  onEditChild,
  onChildMenuClick
}) => {
  return (
    <Paper sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
        <Typography variant="h6">
          Your Children
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={onAddChild}
        >
          Add Child
        </Button>
      </Box>

      {children && children.length > 0 ? (
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: 'repeat(2, 1fr)' }, gap: 2 }}>
          {children.map((child, index) => (
            <ChildCard
              key={child.childId || index}
              child={child}
              onEdit={onEditChild}
              onMenuClick={onChildMenuClick}
            />
          ))}
        </Box>
      ) : (
        <Box sx={{ textAlign: 'center', py: 4 }}>
          <PersonIcon sx={{ fontSize: 48, color: 'text.secondary', mb: 2 }} />
          <Typography variant="body1" color="text.secondary">
            No children registered yet
          </Typography>
          <Typography variant="body2" color="text.secondary">
            Click "Add Child" to get started
          </Typography>
        </Box>
      )}
    </Paper>
  );
};

export default ChildrenList;
