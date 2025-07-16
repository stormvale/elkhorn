
export interface User {
  id: string;
  email: string;
  name: string;
  username?: string;
  roles: string[];
  tenantId?: string;
}