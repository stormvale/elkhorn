import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Container, Typography, Card, CardContent, Button, 
  FormControl, RadioGroup, FormControlLabel, Radio, 
  CircularProgress, Box, Alert 
} from '@mui/material';
import { useAppDispatch } from '../../app/hooks';
import { setCurrentSchool } from '../../app/authSlice';
import { useLinkUserToSchoolMutation, UserSchoolDto } from '../users/api/apiSlice-generated';
import { useAuthContext } from '../../hooks/useAuthContext';
import { useListSchoolsQuery } from '../schools/api/apiSlice';

/*
 * This component allows new users to select their school for
 * the first time after registration.
 */
const SchoolSelector = () => {
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { currentUser } = useAuthContext();
  const [selectedSchoolId, setSelectedSchoolId] = useState<string>('');

  const { data: schools = [], isLoading, error: errorFetching } = useListSchoolsQuery();
  const [linkUserToSchool, { isLoading: isLinking, error: errorLinking }] = useLinkUserToSchoolMutation();

  const handleSelectSchool = async () => {
    await linkUserToSchool({ userId: currentUser!.id, schoolId: selectedSchoolId })
      .then(() => {

        // set the Redux school context for new user
        const selectedSchool = schools.find(s => s.id === selectedSchoolId);
        if (selectedSchool) {
          
          const schoolDto: UserSchoolDto = {
            id: selectedSchool.id,
            name: selectedSchool.name,
            children: []
          };

          dispatch(setCurrentSchool(schoolDto));
          navigate('/home');
        }
      })
  };

  if (!currentUser) {
    return (
      <Container maxWidth="sm" sx={{ py: 4, textAlign: 'center' }}>
        <Typography variant="h6">Please sign in first</Typography>
        <Button onClick={() => navigate('/')} sx={{ mt: 2 }}>
          Go to Sign In
        </Button>
      </Container>
    );
  }

  if (isLoading) {
    return (
      <Container maxWidth="sm" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
          <CircularProgress />
          <Typography>Loading available schools...</Typography>
        </Box>
      </Container>
    );
  }

  if (errorFetching) {
    return (
      <Container maxWidth="sm" sx={{ py: 4 }}>
        <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
          <Typography>Error fetching list of schools...</Typography>
        </Box>
      </Container>
    );
  }

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom textAlign="center">
        Select Your School
      </Typography>
      
      <Typography variant="body1" textAlign="center" color="text.secondary" sx={{ mb: 4 }}>
        Please select the school where your child attends. This will be linked to your profile.
      </Typography>

      {errorLinking && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {'Failed to link school. Please try again.'}
        </Alert>
      )}

      <Card>
        <CardContent sx={{ p: 3 }}>
          <FormControl component="fieldset" fullWidth>
            <RadioGroup
              value={selectedSchoolId}
              onChange={(e) => setSelectedSchoolId(e.target.value)}
            >
              {schools.map((school) => (
                <FormControlLabel
                  key={school.id}
                  value={school.id}
                  control={<Radio />}
                  label={school.name}
                  sx={{ 
                    mb: 1,
                    '& .MuiFormControlLabel-label': {
                      fontSize: '1.1rem'
                    }
                  }}
                />
              ))}
            </RadioGroup>
          </FormControl>

          <Button
            variant="contained"
            size="large"
            fullWidth
            onClick={handleSelectSchool}
            disabled={!selectedSchoolId || isLinking}
            sx={{ mt: 3 }}
          >
            {isLinking ? (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <CircularProgress size={20} color="inherit" />
                Linking...
              </Box>
            ) : (
              'Continue'
            )}
          </Button>
        </CardContent>
      </Card>
    </Container>
  );
};

export default SchoolSelector;