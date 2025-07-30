import React from 'react';
import {
  Box,
  Button,
  Paper,
  Typography
} from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';

interface QuickActionsProps {
  onAddChild: () => void;
  onUpdateChild?: () => void;
  onTransferSchool?: () => void;
}

const QuickActions: React.FC<QuickActionsProps> = ({
  onAddChild,
  onUpdateChild,
  onTransferSchool
}) => {
  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h6" gutterBottom>
        Quick Actions
      </Typography>
      <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        <Button 
          variant="outlined" 
          fullWidth
          startIcon={<AddIcon />}
          onClick={onAddChild}
        >
          Register New Child
        </Button>
        <Button 
          variant="outlined" 
          fullWidth
          disabled={!onUpdateChild}
          onClick={onUpdateChild}
        >
          Update Child Info
        </Button>
        <Button 
          variant="outlined" 
          fullWidth
          disabled={!onTransferSchool}
          onClick={onTransferSchool}
        >
          Transfer School
        </Button>
      </Box>
    </Paper>
  );
};

export default QuickActions;
