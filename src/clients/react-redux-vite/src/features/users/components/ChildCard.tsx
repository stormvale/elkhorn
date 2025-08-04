import React from 'react';
import { Box, Card, CardContent, IconButton, Typography } from '@mui/material';
import {
  Person as PersonIcon,
  Edit as EditIcon,
  MoreVert as MoreVertIcon
} from '@mui/icons-material';
import { UserChild } from '../../../app/authSlice';

interface ChildCardProps {
  child: UserChild;
  onEdit?: (child: UserChild) => void;
  onMenuClick?: (child: UserChild) => void;
}

const ChildCard: React.FC<ChildCardProps> = ({
  child,
  onEdit,
  onMenuClick
}) => {
  return (
    <Card variant="outlined">
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2, flex: 1 }}>
            <PersonIcon color="primary" sx={{ mr: 1 }} />
            <Typography variant="h6">
              {child.firstName} {child.lastName}
            </Typography>
          </Box>
          <Box sx={{ display: 'flex', gap: 0.5 }}>
            {onEdit && (
              <IconButton
                size="small"
                onClick={() => onEdit(child)}
                sx={{ opacity: 0.7, '&:hover': { opacity: 1 } }}
              >
                <EditIcon fontSize="small" />
              </IconButton>
            )}
            {onMenuClick && (
              <IconButton
                size="small"
                onClick={() => onMenuClick(child)}
                sx={{ opacity: 0.7, '&:hover': { opacity: 1 } }}
              >
                <MoreVertIcon fontSize="small" />
              </IconButton>
            )}
          </Box>
        </Box>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
          Grade: {child.grade}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          School: {child.schoolName}
        </Typography>
      </CardContent>
    </Card>
  );
};

export default ChildCard;
