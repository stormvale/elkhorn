import HomePage from '../features/home/HomePage';
import { Restaurants } from '../features/restaurants/Restaurants';
import {
  Home as HomeIcon,
  Restaurant as RestaurantIcon,
} from '@mui/icons-material';

interface RouteConfig {
  path: string;
  element: React.ReactElement;
  icon: React.ReactNode;
  displayName: string;
  requiresAuth: boolean;
  allowedRoles: string[];
}

const sidebarRoutes: RouteConfig[] = [
  {
    path: '/home',
    element: <HomePage />,
    icon: <HomeIcon />,
    displayName: "Home",
    requiresAuth: true,
    allowedRoles: [],
  },
  {
    path: '/restaurants',
    element: <Restaurants />,
    icon: <RestaurantIcon />,
    displayName: "Restaurants",
    requiresAuth: true,
    allowedRoles: [],
  }
  
  // {
  //   path: '/quotes',
  //   element: <Quotes />,
  //   requiresAuth: true,
  //   allowedRoles: ['User'],
  // },
  // {
  //   path: '/theme',
  //   element: <TemplateTester />,
  //   requiresAuth: true,
  //   allowedRoles: ['User', 'Admin'],
  // }
];

export default sidebarRoutes;