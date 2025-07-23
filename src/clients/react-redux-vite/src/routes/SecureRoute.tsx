// SecureRoute.tsx
import { Navigate, useLocation } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { ReactElement } from 'react';
import { RootState } from '../app/store';

interface SecureRouteProps {
  children: ReactElement;
  allowedRoles?: string[];
}

export const SecureRoute = ({ children, allowedRoles }: SecureRouteProps) => {
  const { isAuthenticated, user } = useSelector((state: RootState) => state.auth);
  const location = useLocation();

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // Check if user has at least one of the required roles
  if (allowedRoles && allowedRoles.length > 0) {

    // if the user has no roles, then no access
    if (!user || !user.roles || user.roles.length === 0) {
      return <Navigate to="/unauthorized" replace />;
    }

    // check if user has ANY of the allowed roles
    const hasAllowedRole = user.roles.some((userRole: string) => allowedRoles.includes(userRole)
    );

    if (!hasAllowedRole) {
      return <Navigate to="/unauthorized" replace />;
    }
  }

  return <>{children}</>;
};