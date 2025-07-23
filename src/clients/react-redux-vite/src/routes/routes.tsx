import HomePage from '../features/home/HomePage';
import { Restaurants } from '../features/restaurants/Restaurants';
import AuthLanding from '../features/auth/AuthLanding';
import PostLoginHandler from '../features/auth/PostLoginHandler';
import AuthRedirect from '../features/auth/AuthRedirect';

interface RouteConfig {
  path: string;
  element: React.ReactElement;
  requiresAuth: boolean;
  allowedRoles: string[];
}

const routes: RouteConfig[] = [
  {
    path: '/',
    element: <AuthLanding />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/login',
    element: <AuthLanding />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/signin-oidc',
    element: <AuthRedirect />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/post-login',
    element: <PostLoginHandler />,
    requiresAuth: false,
    allowedRoles: [],
  },
  {
    path: '/home',
    element: <HomePage />,
    requiresAuth: true,
    allowedRoles: [],
  },
  {
    path: '/restaurants',
    element: <Restaurants />,
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

export default routes;