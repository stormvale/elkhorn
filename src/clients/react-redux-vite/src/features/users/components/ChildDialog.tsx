import React, { useState, useEffect } from 'react';
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
import { 
  useRegisterChildMutation, 
  useUpdateChildMutation,
  ChildUpsertRequest, 
  ChildResponse
} from '../api/apiSlice-generated';
import { UserChild } from '../../../app/authSlice';

interface ChildDialogProps {
  open: boolean;
  userId: string;
  child?: UserChild; // If provided, we're in edit mode
  onClose: () => void;
  onSuccess: (newChild: ChildResponse) => void;
}

const ChildDialog: React.FC<ChildDialogProps> = ({
  open,
  userId,
  child,
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

  const isEditMode = !!child;

  // API hooks
  const { data: schools = [], isLoading: schoolsLoading } = useListSchoolsQuery();
  const [registerChild, { isLoading: isRegistering }] = useRegisterChildMutation();
  const [updateChild, { isLoading: isUpdating }] = useUpdateChildMutation();

  // Populate form when editing
  useEffect(() => {
    if (child && open) {
      setChildForm({
        firstName: child.firstName,
        lastName: child.lastName,
        schoolId: child.schoolId,
        schoolName: child.schoolName,
        grade: child.grade
      });
    } else if (!isEditMode && open) {
      // Reset form for new child
      setChildForm({
        firstName: '',
        lastName: '',
        schoolId: '',
        schoolName: '',
        grade: ''
      });
    }
  }, [child, open, isEditMode]);

  const handleSubmit = async () => {
    try {
      if (isEditMode && child) {
        await updateChild({ userId, childId: child.childId, childUpsertRequest: childForm }).unwrap();
        onClose();
      } else {
        // new child
        const result = await registerChild({userId, childUpsertRequest: childForm}).unwrap();

        setChildForm({
          firstName: '',
          lastName: '',
          schoolId: '',
          schoolName: '',
          grade: ''
        });

        onSuccess?.(result);
      }
    } catch (error) {
      console.error(`Failed to ${isEditMode ? 'update' : 'register'} child:`, error);
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
    if (!isEditMode) {
      setChildForm({
        firstName: '',
        lastName: '',
        schoolId: '',
        schoolName: '',
        grade: ''
      });
    }
    onClose();
  };

  const isFormValid = childForm.firstName.trim() && 
                     childForm.lastName.trim() && 
                     childForm.schoolId && 
                     childForm.grade.trim();

  const isLoading = isRegistering || isUpdating;

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>
        {isEditMode ? 'Edit Child' : 'Register New Child'}
      </DialogTitle>
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
          disabled={isLoading || !isFormValid}
        >
          {isLoading 
            ? (isEditMode ? 'Saving...' : 'Registering...') 
            : (isEditMode ? 'Save Changes' : 'Register Child')
          }
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default ChildDialog;
