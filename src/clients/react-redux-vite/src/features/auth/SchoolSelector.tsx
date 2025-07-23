import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { 
  Container, Typography, Card, CardContent, Button, 
  FormControl, RadioGroup, FormControlLabel, Radio, 
  CircularProgress, Box, Alert 
} from '@mui/material';
import { useAppDispatch } from '../../app/hooks';
import { setCurrentSchool } from '../../app/authSlice';
import { useSchoolContext } from '../../hooks/useApp';
import { userService } from '../../services/userService';

import { UserSchoolDto } from '../users/api/apiSlice-generated';

interface School {
  id: string;
  name: string;
}

const SchoolSelector = () => {
  const { accounts } = useMsal();
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
  const { currentSchool, availableSchools } = useSchoolContext();
  const [searchParams] = useSearchParams();
  const [schools, setSchools] = useState<School[]>([]);
  const [selectedSchoolId, setSelectedSchoolId] = useState<string>('');
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string>('');

  // this prob shouldn't be a whole page - change to modal dialog later
  
  // Check if this is a school switch operation vs initial selection
  const isSwitching = searchParams.get('mode') === 'switch';

  useEffect(() => {
    const loadSchools = async () => {
      try {
        let schoolsToShow: School[];
        
        if (isSwitching) {
          // User is switching schools - use available schools from Redux
          schoolsToShow = availableSchools.map(school => ({
            id: school.id,
            name: school.name
          }));
          
          // Pre-select current school
          if (currentSchool) {
            setSelectedSchoolId(currentSchool.id);
          }
        } else {
          // New user - show all available schools
          schoolsToShow = await userService.getAvailableSchools();
        }
        
        setSchools(schoolsToShow);
      } catch (error) {
        console.error('Failed to load schools:', error);
        setError('Failed to load available schools. Please try again.');
      } finally {
        setIsLoading(false);
      }
    };

    loadSchools();
  }, [isSwitching, availableSchools, currentSchool]);

  const handleSelectSchool = async () => {
    console.log('SchoolSelector handleSelectSchool:', { 
      selectedSchoolId, 
      isSwitching, 
      schoolsCount: schools.length,
      availableSchoolsCount: availableSchools.length 
    });

    if (!selectedSchoolId) {
      setError('Please select a school before continuing.');
      return;
    }

    if (accounts.length === 0) {
      navigate('/');
      return;
    }

    setIsSaving(true);
    setError('');

    try {
      console.log('About to process school selection...');
      
      if (isSwitching) {
        // User is switching between their existing schools
        console.log('Switching to existing school:', selectedSchoolId);
        const selectedSchool = availableSchools.find(s => s.id === selectedSchoolId);
        if (selectedSchool) {
          console.log('Dispatching setCurrentSchool for switch:', selectedSchool);
          dispatch(setCurrentSchool(selectedSchool));
        }
      } else {
        // New user selecting their first school. When api is ready, link school:
        // await userService.linkSchoolToUser(tokenResponse.accessToken, selectedSchoolId);

        console.log('Setting school for new user:', selectedSchoolId);
        // Set up Redux school context for new user
        const selectedSchool = schools.find(s => s.id === selectedSchoolId);
        if (selectedSchool) {
          // Create a UserSchoolDto-compatible object
          const schoolDto: UserSchoolDto = {
            id: selectedSchool.id,
            name: selectedSchool.name,
            children: [] // Empty for now, will be populated when profile is fetched
          };
          console.log('Dispatching setCurrentSchool for new user:', schoolDto);
          dispatch(setCurrentSchool(schoolDto));
        }
      }

      console.log('Navigating to /home');
      // Navigate to home
      navigate('/home');
      
    } catch (error) {
      console.error('Failed to select school:', error);
      setError(`Failed to ${isSwitching ? 'switch to' : 'select'} school. Please try again.`);
    } finally {
      setIsSaving(false);
    }
  };

  if (!accounts.length) {
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

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Typography variant="h4" component="h1" gutterBottom textAlign="center">
        {isSwitching ? 'Switch School' : 'Select Your School'}
      </Typography>
      
      <Typography variant="body1" textAlign="center" color="text.secondary" sx={{ mb: 4 }}>
        {isSwitching 
          ? 'Choose which school context you\'d like to work with.'
          : 'Please select the school where your child attends. This will be linked to your profile.'
        }
      </Typography>

      {error && (
        <Alert severity="error" sx={{ mb: 3 }}>
          {error}
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
            disabled={!selectedSchoolId || isSaving}
            sx={{ mt: 3 }}
          >
            {isSaving ? (
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <CircularProgress size={20} color="inherit" />
                {isSwitching ? 'Switching...' : 'Saving...'}
              </Box>
            ) : (
              isSwitching ? 'Switch School' : 'Continue'
            )}
          </Button>
        </CardContent>
      </Card>
    </Container>
  );
};

export default SchoolSelector;