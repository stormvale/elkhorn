import { useState } from "react";
import { useMsal } from "@azure/msal-react";
import { useNavigate } from "react-router-dom";
import { 
  Typography, Stack, Container, Card, CardContent, Button, 
  Box, CircularProgress, Alert 
} from '@mui/material';
import { loginRequest } from "../../msalConfig";
import { useAuthenticatedUser } from "../../hooks/useApp";

const AuthLanding = () => {
  const [isLoading, setIsLoading] = useState(false);
  const { instance } = useMsal();
  const { user, isAuthenticated } = useAuthenticatedUser();
  const navigate = useNavigate();

  const handleLogin = async () => {
    setIsLoading(true);
    try {
      await instance.loginRedirect(loginRequest);
    } catch (error) {
      console.error("Login failed:", error);
      setIsLoading(false);
    }
  };

  const handleContinueToApp = () => {
    // Navigate to home or school selector based on user's school context
    navigate('/home');
  };

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Stack spacing={3} alignItems="center">
        <Typography variant="h3" component="h1" textAlign="center">
          Project: Elkhorn
        </Typography>
        
        <Typography variant="h6" textAlign="center" color="text.secondary">
          Your school communication hub
        </Typography>

        {isAuthenticated && user && (
          <Alert severity="success" sx={{ width: '100%' }}>
            <Box>
              <Typography variant="body1" fontWeight="medium">
                Welcome back, {user.name || user.email}!
              </Typography>
            </Box>
          </Alert>
        )}

        <Card sx={{ width: '100%', mt: 4 }}>
          <CardContent sx={{ p: 4 }}>
            <Stack spacing={3} alignItems="center">
              <Typography variant="h5" textAlign="center">
                {isAuthenticated ? 'Welcome Back!' : 'Sign In or Create Account'}
              </Typography>
              
              <Typography textAlign="center" color="text.secondary">
                {isAuthenticated 
                  ? 'Continue to your school communication hub' 
                  : 'Connect with your school using your preferred account'
                }
              </Typography>

              {isAuthenticated ? (
                <Button
                  variant="contained"
                  size="large"
                  onClick={handleContinueToApp}
                  sx={{ minWidth: 200 }}
                >
                  Continue to App
                </Button>
              ) : (
                <Button
                  variant="contained"
                  size="large"
                  onClick={handleLogin}
                  disabled={isLoading}
                  sx={{ minWidth: 200 }}
                >
                  {isLoading ? (
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                      <CircularProgress size={20} color="inherit" />
                      Signing in...
                    </Box>
                  ) : (
                    'Continue with Social Account'
                  )}
                </Button>
              )}
              
              {!isAuthenticated && (
                <Typography variant="body2" textAlign="center" color="text.secondary">
                  Sign in or create an account using Google, Facebook, or Microsoft
                </Typography>
              )}
            </Stack>
          </CardContent>
        </Card>
      </Stack>
    </Container>
  );
}

export default AuthLanding;
