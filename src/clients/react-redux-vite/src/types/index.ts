export interface User {
  id: string;
  email: string;
  name: string;
  username?: string;
  roles: string[];
  tenantId?: string;
  localAccountId?: string;
  environment?: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  user: User | null;
  accessToken: string | null;
  loading: boolean;
  error: string | null;
}

export interface MSALTokenClaims {
  preferred_username?: string;
  name?: string;
  email?: string;
  oid?: string;
  tid?: string;
  roles?: string[];
}