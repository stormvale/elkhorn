import React, { useState } from 'react';
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography
} from '@mui/material';
import { useListSchoolsQuery } from '../../schools/api/apiSlice-generated';
import { useRegisterChildMutation, ChildUpsertRequest } from '../api/apiSlice-generated';

interface RegisterChildDialogProps {
  open: boolean;
  userId: string;
  onClose: () => void;
  onSuccess?: () => void;
}

const RegisterChildDialog: React.FC<RegisterChildDialogProps> = ({
  open,
  userId,
  onClose,
  onSuccess
}) => {
  const [childForm, setChildForm] = useState<ChildUpsertRequest>({
    firstName: '',
    lastName: '',
    schoolId: '',
    schoolName: '',
    grade: ''
  });

  // API hooks
  const { data: schools = [], isLoading: schoolsLoading } = useListSchoolsQuery();
  const [registerChild, { isLoading: isRegistering }] = useRegisterChildMutation();

  const handleSubmit = async () => {
    try {
      await registerChild({ userId, childUpsertRequest: childForm }).unwrap();
      
      // Reset form
      setChildForm({
        firstName: '',
        lastName: '',
        schoolId: '',
        schoolName: '',
        grade: ''
      });
      
      onClose();
      onSuccess?.();
    } catch (error) {
      console.error('Failed to register child:', error);
    }
  };

  const handleSchoolChange = (schoolId: string) => {
    const selectedSchool = schools.find(school => school.id === schoolId);
    setChildForm({
      ...childForm,
      schoolId,
      schoolName: selectedSchool?.name || ''
    });
  };

  const handleClose = () => {
    // Reset form when closing
    setChildForm({
      firstName: '',
      lastName: '',
      schoolId: '',
      schoolName: '',
      grade: ''
    });
    onClose();
  };

  const isFormValid = childForm.firstName.trim() && 
                     childForm.lastName.trim() && 
                     childForm.schoolId && 
                     childForm.grade.trim();

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>Register New Child</DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, pt: 1 }}>
          <Box sx={{ display: 'flex', gap: 2 }}>
            <TextField
              label="First Name"
              value={childForm.firstName}
              onChange={(e) => setChildForm({ ...childForm, firstName: e.target.value })}
              fullWidth
              required
            />
            <TextField
              label="Last Name"
              value={childForm.lastName}
              onChange={(e) => setChildForm({ ...childForm, lastName: e.target.value })}
              fullWidth
              required
            />
          </Box>

          <FormControl fullWidth required>
            <InputLabel>School</InputLabel>
            <Select
              value={childForm.schoolId}
              label="School"
              onChange={(e) => handleSchoolChange(e.target.value)}
              disabled={schoolsLoading}
            >
              {schoolsLoading ? (
                <MenuItem disabled>Loading schools...</MenuItem>
              ) : schools.length === 0 ? (
                <MenuItem disabled>No schools available</MenuItem>
              ) : (
                schools.map((school) => (
                  <MenuItem key={school.id} value={school.id}>
                    {school.name}
                  </MenuItem>
                ))
              )}
            </Select>
          </FormControl>

          <TextField
            label="Grade"
            value={childForm.grade}
            onChange={(e) => setChildForm({ ...childForm, grade: e.target.value })}
            fullWidth
            required
            placeholder="e.g., Grade 3, Kindergarten"
          />

          {schoolsLoading && (
            <Typography variant="body2" color="text.secondary">
              Loading available schools...
            </Typography>
          )}
        </Box>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Cancel</Button>
        <Button 
          onClick={handleSubmit} 
          variant="contained"
          disabled={isRegistering || !isFormValid}
        >
          {isRegistering ? 'Registering...' : 'Register Child'}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default RegisterChildDialog;
