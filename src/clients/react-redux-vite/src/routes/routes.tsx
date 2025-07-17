import LoginPage from '../features/auth/LoginPage';
import HomePage from '../features/home/HomePage';
import { Restaurants } from '../features/restaurants/Restaurants';
import { Quotes } from '../features/quotes/Quotes';
import TemplateTester from '../features/theme/TemplateTester';

interface RouteConfig {
  path: string;
  element: React.ReactElement;
  requiresAuth: boolean;
  allowedRoles: string[];
}

const routes: RouteConfig[] = [
  {
    path: '/',
    element: <LoginPage />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/login',
    element: <LoginPage />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/home',
    element: <HomePage />,
    requiresAuth: true,
    allowedRoles: ['User'],
  },
  {
    path: '/restaurants',
    element: <Restaurants />,
    requiresAuth: true,
    allowedRoles: ['User', 'Admin'],
  },
  {
    path: '/quotes',
    element: <Quotes />,
    requiresAuth: true,
    allowedRoles: ['User'],
  },
  {
    path: '/theme',
    element: <TemplateTester />,
    requiresAuth: true,
    allowedRoles: ['User', 'Admin'],
  }
];

export default routes;