import React, { useState } from 'react';
import { Chip, Menu, MenuItem, Typography, Box, Tooltip} from '@mui/material';
import { School as SchoolIcon, ExpandMore as ExpandMoreIcon } from '@mui/icons-material';
import { useAuthContext } from '../hooks/useAuthContext';
import { setCurrentSchool, UserSchool } from '../app/authSlice';

interface SchoolChipProps {
  size?: 'small' | 'medium';
  variant?: 'filled' | 'outlined';
}

const SchoolChip: React.FC<SchoolChipProps> = ({ size = 'medium', variant = 'outlined'}) => {
  const { currentUser, currentSchool } = useAuthContext();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const hasMultipleSchools = currentUser!.schools.length > 1;

  const currentSchoolChildren = currentUser?.children?.filter(child => 
    child.schoolId === currentSchool?.schoolId
  ) || [];

  // tooltip content
  const tooltipContent = (
    <Box>
      {currentSchoolChildren.map((child, index) => (
        <Typography key={child.childId || index} variant="caption" display="block">
          {`${child.firstName} ${child.lastName} (${child.grade})`}
        </Typography>
      ))}
    </Box>
  );

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    if (hasMultipleSchools) {
      setAnchorEl(event.currentTarget);
    }
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleSchoolSelect = (school: UserSchool) => {
    setCurrentSchool(school);
    handleClose();
  };

  if (!currentSchool) {
    return null;
  }

  return (
    <>
      <Tooltip 
        title={tooltipContent} 
        placement="right"
        arrow
        enterDelay={500}
        leaveDelay={200}
      >
        <Chip
          icon={<SchoolIcon />}
          label={
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
              {currentSchool.schoolName.replace('Elementary', '')}
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
      </Tooltip>
      
      {hasMultipleSchools && (
        <Menu anchorEl={anchorEl} open={open} onClose={handleClose}>
          {currentUser!.schools.map((school) => (
            <MenuItem
              key={school.schoolId}
              onClick={() => handleSchoolSelect(school)}
              selected={school === currentSchool}
            >
              <Box>
                <Typography variant="body2" fontWeight="medium">
                  {school.schoolName}
                </Typography>
              </Box>
            </MenuItem>
          ))}
        </Menu>
      )}
    </>
  );
};

export default SchoolChip;
