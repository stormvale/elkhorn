import HomePage from '../features/home/HomePage';
import RestaurantsPage from '../features/restaurants/RestaurantsPage';
import {
  Home as HomeIcon,
  Restaurant as RestaurantIcon,
  Group as GroupIcon,
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
  }
];

// Section configuration
export const sidebarSections = {
  pac: {
    title: 'PAC',
    icon: <GroupIcon />,
    defaultOpen: false
  }
};

export default sidebarRoutes;