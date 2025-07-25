import { useEffect, useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useMsal } from '@azure/msal-react';
import { 
  Container, Typography, Card, CardContent, Button, 
  FormControl, RadioGroup, FormControlLabel, Radio, 
  CircularProgress, Box, Alert 
} from '@mui/material';
import { userService } from '../../services/userService';
import { schoolContextService } from '../../services/schoolContextService';

interface School {
  id: string;
  name: string;
}

const SchoolSelector = () => {
  const { accounts } = useMsal();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [schools, setSchools] = useState<School[]>([]);
  const [selectedSchoolId, setSelectedSchoolId] = useState<string>('');
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);
  const [error, setError] = useState<string>('');
  
  // Check if this is a school switch operation vs initial selection
  const isSwitching = searchParams.get('mode') === 'switch';

  useEffect(() => {
    const loadSchools = async () => {
      try {
        let availableSchools: School[];
        
        if (isSwitching) {
          // User is switching schools - only show their linked schools
          const userSchools = schoolContextService.getUserSchools();
          availableSchools = userSchools.map(school => ({
            id: school.id,
            name: school.name
          }));
          
          // Pre-select current school
          const currentSchool = schoolContextService.getCurrentSchool();
          if (currentSchool) {
            setSelectedSchoolId(currentSchool.id);
          }
        } else {
          // New user - show all available schools
          availableSchools = await userService.getAvailableSchools();
        }
        
        setSchools(availableSchools);
      } catch (error) {
        console.error('Failed to load schools:', error);
        setError('Failed to load available schools. Please try again.');
      } finally {
        setIsLoading(false);
      }
    };

    loadSchools();
  }, [isSwitching]);

  const handleSelectSchool = async () => {
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
      if (isSwitching) {
        // User is switching between their existing schools
        await schoolContextService.switchSchool(selectedSchoolId);
      } else {
        // New user selecting their first school
        // When backend is ready, get access token and link school:
        // const tokenResponse = await instance.acquireTokenSilent({
        //   ...apiRequest,
        //   account: accounts[0]
        // });
        // await userService.linkSchoolToUser(tokenResponse.accessToken, selectedSchoolId);

        // Set up local school context for new user
        const selectedSchool = schools.find(s => s.id === selectedSchoolId);
        if (selectedSchool) {
          // Create user school object
          // const userSchool = {
          //   id: selectedSchool.id,
          //   name: selectedSchool.name,
          //   children: [] // This would come from API in real implementation
          // };
          
          //schoolContextService.setUserSchools([userSchool]);
          schoolContextService.setCurrentSchool(selectedSchoolId);
        }
      }

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