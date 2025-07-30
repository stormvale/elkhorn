import React, { useState } from 'react';
import { 
  Container, 
  Box,
  Typography
} from '@mui/material';
import { useAuthContext } from '../../hooks/useAuthContext';
import { UserChild } from '../../app/authSlice';
import { useGetUserByIdQuery } from './api/apiSlice-generated';
import { ChildrenList, QuickActions, ChildDialog } from './components';

const ManageChildren: React.FC = () => {
  const { currentUser } = useAuthContext();
  const [showChildDialog, setShowChildDialog] = useState(false);
  const [editingChild, setEditingChild] = useState<UserChild | undefined>(undefined);

  // Use the API query to get fresh user data and enable refetching
  const { 
    data: userData, 
    refetch: refetchUser 
  } = useGetUserByIdQuery(currentUser?.id || '', {
    skip: !currentUser?.id, // Skip the query if no user ID
  });

  // Use the API data if available, fallback to auth context data
  const displayUser = userData || currentUser;

  const handleAddChild = () => {
    setEditingChild(undefined);
    setShowChildDialog(true);
  };

  const handleEditChild = (child: UserChild) => {
    setEditingChild(child);
    setShowChildDialog(true);
  };

  const handleChildMenuClick = (child: UserChild) => {
    // TODO: Implement child menu functionality
    console.log('Child menu clicked:', child);
  };

  const handleRegisterSuccess = () => {
    // Refetch user data to get the updated children list
    refetchUser();
    setShowChildDialog(false);
    setEditingChild(undefined);
    console.log('Child registered successfully');
  };

  return (
    <Container maxWidth="lg" sx={{ py: 4 }}>
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Manage Children
        </Typography>
        <Typography variant="body1" color="text.secondary">
          Add or update information about your children
        </Typography>
      </Box>

      <Box sx={{ display: 'flex', gap: 3, flexDirection: { xs: 'column', md: 'row' } }}>
        {/* Current Children */}
        <Box sx={{ flex: { md: 2 } }}>
          <ChildrenList
            children={displayUser?.children || []}
            onAddChild={handleAddChild}
            onEditChild={handleEditChild}
            onChildMenuClick={handleChildMenuClick}
          />
        </Box>

        {/* Quick Actions */}
        <Box sx={{ flex: { md: 1 } }}>
          <QuickActions
            onAddChild={handleAddChild}
            // onUpdateChild={handleUpdateChild} // TODO: Implement
            // onTransferSchool={handleTransferSchool} // TODO: Implement
          />
        </Box>
      </Box>

      {/* Child Dialog for Add/Edit */}
      <ChildDialog
        open={showChildDialog}
        child={editingChild}
        userId={displayUser?.id || ''}
        onClose={() => {
          setShowChildDialog(false);
          setEditingChild(undefined);
        }}
        onSuccess={handleRegisterSuccess}
      />
    </Container>
  );
};

export default ManageChildren;
