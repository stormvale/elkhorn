import HomePage from '../features/home/HomePage';
import RestaurantsPage from '../features/restaurants/RestaurantsPage';
import ManageChildren from '../features/users/ManageChildren';
import {
  Home as HomeIcon,
  Restaurant as RestaurantIcon,
  Group as GroupIcon,
  Settings as SettingsIcon,
  Person as PersonIcon,
} from '@mui/icons-material';

interface RouteConfig {
  path: string;
  element: React.ReactElement;
  icon: React.ReactNode;
  displayName: string;
  requiresAuth: boolean;
  allowedRoles: string[];
  section?: string; // New field for grouping
}

const sidebarRoutes: RouteConfig[] = [
  // Standalone Home
  {
    path: '/home',
    element: <HomePage />,
    icon: <HomeIcon />,
    displayName: "Home",
    requiresAuth: true,
    allowedRoles: [],
  },
  
  // PAC Section
  {
    path: '/restaurants',
    element: <RestaurantsPage />,
    icon: <RestaurantIcon />,
    displayName: "Restaurants",
    requiresAuth: true,
    allowedRoles: [],
    section: 'pac'
  },
  
  // User Settings Section
  {
    path: '/manage-children',
    element: <ManageChildren />,
    icon: <PersonIcon />,
    displayName: "Manage Children",
    requiresAuth: true,
    allowedRoles: [],
    section: 'settings'
  }
];

// Section configuration
export const sidebarSections = {
  pac: {
    title: 'PAC',
    icon: <GroupIcon />,
    defaultOpen: false
  },
  settings: {
    title: 'User Settings',
    icon: <SettingsIcon />,
    defaultOpen: false
  }
};

export default sidebarRoutes;