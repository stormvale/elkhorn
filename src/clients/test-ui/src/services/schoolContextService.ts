import { msalInstance } from "../msalConfig";

export interface UserSchool {
  schoolId: string;
  schoolName: string;
  roles: string[];
  children: {
    id: string;
    name: string;
    grade: string;
  }[];
}

export class SchoolContextService {
  private static instance: SchoolContextService;
  private currentSchoolId: string | null = null;
  private userSchools: UserSchool[] = [];

  private constructor() {}

  static getInstance(): SchoolContextService {
    if (!SchoolContextService.instance) {
      SchoolContextService.instance = new SchoolContextService();
    }
    return SchoolContextService.instance;
  }

  /**
   * Initialize the service after authentication
   */
  async initialize(): Promise<void> {
    const account = msalInstance.getActiveAccount();
    if (!account) {
      throw new Error("No active account found");
    }

    try {
      // // Fetch user's school associations from the API
      // const response = await fetch(`/api/users/${account.localAccountId}/schools`, {
      //   headers: {
      //     'Authorization': `Bearer ${await this.getAccessToken()}`
      //   }
      // });

      // if (!response.ok) {
      //   throw new Error("Failed to fetch user schools");
      // }

      // this.userSchools = await response.json();
      this.userSchools = [
        {
          schoolId: "221",
          schoolName: "Fairview Elementary",
          roles: ["Parent/Guardian", "PAC Admin"],
          children: [
            { id: "child1", name: "Timmy Smith", grade: "3rd Grade" }
          ]
        },
        {
          schoolId: "291",
          schoolName: "Rock City Elementary",
          roles: ["Parent/Guardian"],
          children: [
            { id: "child2", name: "Sally Smith", grade: "2nd Grade" }
          ]
        }
      ];
      
      // Set initial school context
      const storedSchoolId = sessionStorage.getItem("schoolId");
      if (storedSchoolId && this.userSchools.some(s => s.schoolId === storedSchoolId)) {
        this.currentSchoolId = storedSchoolId;
      } else if (this.userSchools.length === 1) {
        // If user only has access to one school, auto-select it
        this.currentSchoolId = this.userSchools[0].schoolId;
      }
    } catch (error) {
      console.error("Failed to initialize school context:", error);
    }
  }

  /**
   * Get all schools the current user has access to
   */
  getUserSchools(): UserSchool[] {
    return this.userSchools;
  }

  /**
   * Get the currently selected school
   */
  getCurrentSchool(): UserSchool | null {
    if (!this.currentSchoolId) return null;
    return this.userSchools.find(s => s.schoolId === this.currentSchoolId) || null;
  }

  /**
   * Switch to a different school context
   */
  async switchSchool(schoolId: string): Promise<void> {
    const school = this.userSchools.find(s => s.schoolId === schoolId);
    if (!school) {
      throw new Error("User does not have access to this school");
    }

    this.currentSchoolId = schoolId;
    sessionStorage.setItem("schoolId", schoolId);
    
    // Optionally, notify your backend about the context switch
    try {
      await fetch('/api/user/context', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${await this.getAccessToken()}`
        },
        body: JSON.stringify({ schoolId })
      });
    } catch (error) {
      console.warn("Failed to notify backend of context switch:", error);
    }
  }

  /**
   * Check if user has access to multiple schools
   */
  hasMultipleSchools(): boolean {
    return this.userSchools.length > 1;
  }

  /**
   * Get access token for API calls
   */
  private async getAccessToken(): Promise<string> {
    const account = msalInstance.getActiveAccount();
    if (!account) {
      throw new Error("No active account");
    }

    const response = await msalInstance.acquireTokenSilent({
      scopes: ["api://f776afca-bc47-4fee-9c85-e86ee08578f5/RestaurantsApi.All"],
      account
    });

    return response.accessToken;
  }

  /**
   * Clear the current context (for logout)
   */
  clear(): void {
    this.currentSchoolId = null;
    this.userSchools = [];
    sessionStorage.removeItem("schoolId");
  }
}

export const schoolContextService = SchoolContextService.getInstance();
