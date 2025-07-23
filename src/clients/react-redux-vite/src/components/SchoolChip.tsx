import React, { useState } from 'react';
import { Chip, Menu, MenuItem, Typography, Box} from '@mui/material';
import { School as SchoolIcon, ExpandMore as ExpandMoreIcon } from '@mui/icons-material';
import { useSchoolContext } from '../hooks/useApp';

interface SchoolChipProps {
  size?: 'small' | 'medium';
  variant?: 'filled' | 'outlined';
}

const SchoolChip: React.FC<SchoolChipProps> = ({
  size = 'medium',
  variant = 'outlined'
}) => {
  const { currentSchool, availableSchools, switchSchool, hasMultipleSchools } = useSchoolContext();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (hasMultipleSchools) {
      setAnchorEl(event.currentTarget);
    }
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleSchoolSelect = (schoolId: string) => {
    const selectedSchool = availableSchools.find(school => school.id === schoolId);
    if (selectedSchool) {
      switchSchool(selectedSchool);
    }
    handleClose();
  };

  if (!currentSchool) {
    return null;
  }

  return (
    <>
      <Chip
        icon={<SchoolIcon />}
        label={
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
            {currentSchool.name.replace('Elementary', '')}
            {hasMultipleSchools && <ExpandMoreIcon sx={{ fontSize: 16 }} />}
          </Box>
        }
        onClick={handleClick}
        size={size}
        variant={variant}
        sx={{
          cursor: hasMultipleSchools ? 'pointer' : 'default',
          bgcolor: 'primary.main',
          color: 'white',
          '& .MuiChip-icon': {
            color: 'white'
          },
          '&:hover': hasMultipleSchools ? {
            backgroundColor: 'primary.dark',
          } : {}
        }}
      />
      
      {hasMultipleSchools && (
        <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
          {availableSchools.map((school) => (
            <MenuItem
              key={school.id}
              onClick={() => handleSchoolSelect(school.id)}
              selected={school.id === currentSchool?.id}
            >
              <Box>
                <Typography variant="body2" fontWeight="medium">
                  {school.name}
                </Typography>
                {school.children && school.children.length > 0 && (
                  <Typography variant="caption">
                    {school.children.length} child{school.children.length !== 1 ? 'ren' : ''}
                  </Typography>
                )}
              </Box>
            </MenuItem>
          ))}
        </Menu>
      )}
    </>
  );
};

export default SchoolChip;
