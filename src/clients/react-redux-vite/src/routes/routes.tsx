import LoginPage from '../features/auth/LoginPage';
import HomePage from '../features/home/HomePage';

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
  }
];

export default routes;