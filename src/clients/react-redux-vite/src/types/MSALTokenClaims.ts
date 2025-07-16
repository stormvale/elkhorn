
export interface MSALTokenClaims {
  preferred_username?: string;
  name?: string;
  email?: string;
  oid?: string;
  tid?: string;
  roles?: string[];
}