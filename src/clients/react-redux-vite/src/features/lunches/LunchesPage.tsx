import React, { useState } from 'react';
import { Container, Typography, Box, Alert, Button } from '@mui/material';
import { Add as AddIcon } from '@mui/icons-material';
import { useAuthContext } from '../../hooks/useAuthContext';
import { useListLunchesQuery, useCancelLunchMutation } from './api/apiSlice';
import { useListRestaurantsQuery } from '../restaurants/api/apiSlice-generated';
import { LunchDialog, LunchesGrid } from './components';
import { LunchResponse } from './api/apiSlice-generated';

const LunchesPage: React.FC = () => {
  const { currentUser, currentSchool } = useAuthContext();
  const [selectedLunch, setSelectedLunch] = useState<LunchResponse | null>(null);
  const [showLunchDialog, setShowLunchDialog] = useState(false);

  // API hooks
  const { data: lunches = [], isLoading: lunchesLoading, error: lunchesError } = useListLunchesQuery(undefined);
  const { data: restaurants = [], isLoading: restaurantsLoading, error: restaurantsError } = useListRestaurantsQuery();
  const [deleteLunch, { isLoading: isDeleting }] = useCancelLunchMutation();

  // Check if user is PacAdmin
  const isPacAdmin = currentUser?.roles?.includes('PacAdmin');

  const handleAddLunch = () => {
    setSelectedLunch(null);
    setShowLunchDialog(true);
  };

  const handleEditLunch = (lunch: LunchResponse) => {
    setSelectedLunch(lunch);
    setShowLunchDialog(true);
  };

  const handleDeleteLunch = async (lunchId: string) => {
    if (confirm('Are you sure you want to delete this lunch?')) {
      try {
        await deleteLunch(lunchId).unwrap();
      } catch (error) {
        console.error('Failed to delete lunch:', error);
      }
    }
  };

  const handleLunchSuccess = () => {
    setShowLunchDialog(false);
    setSelectedLunch(null);
  };

  if (!isPacAdmin) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">
          Access denied. Only PacAdmin users can manage lunches.
        </Alert>
      </Container>
    );
  }

  if (!currentSchool) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="warning">
          Please select a school to manage lunches.
        </Alert>
      </Container>
    );
  }

  if (lunchesLoading || restaurantsLoading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Typography>Loading lunches...</Typography>
      </Container>
    );
  }

  if (lunchesError || restaurantsError) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Alert severity="error">
          Failed to load data: 
          {lunchesError && <div>Lunches: {JSON.stringify(lunchesError)}</div>}
          {restaurantsError && <div>Restaurants: {JSON.stringify(restaurantsError)}</div>}
        </Alert>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Lunch Management
        </Typography>
        <Typography variant="body1" color="text.secondary" gutterBottom>
          Schedule and manage lunches for {currentSchool.schoolName}
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAddLunch}
          sx={{ mt: 2 }}
        >
          Schedule New Lunch
        </Button>
      </Box>

      <LunchesGrid
        lunches={lunches}
        restaurants={restaurants}
        currentSchool={currentSchool}
        isDeleting={isDeleting}
        onEditLunch={handleEditLunch}
        onDeleteLunch={handleDeleteLunch}
      />

      {/* Lunch Dialog */}
      <LunchDialog
        open={showLunchDialog}
        lunch={selectedLunch}
        restaurants={restaurants}
        currentSchool={currentSchool}
        onClose={() => {
          setShowLunchDialog(false);
          setSelectedLunch(null);
        }}
        onSuccess={handleLunchSuccess}
      />
    </Container>
  );
};

export default LunchesPage;
