// export interface UserSchool {
//   id: string;
//   name: string;
//   children: {
//     id: string;
//     name: string;
//     grade: string;
//   }[];
// }

import { UserSchoolDto } from "../features/users/api/apiSlice-generated";

export class SchoolContextService {
  private static instance: SchoolContextService;
  private currentSchoolId: string | null = null;
  private userSchools: UserSchoolDto[] = [];

  private constructor() {}

  static getInstance(): SchoolContextService {
    if (!SchoolContextService.instance) {
      SchoolContextService.instance = new SchoolContextService();
    }
    return SchoolContextService.instance;
  }

  /**
   * Initialize the service - this will get the current school from session storage
   */
  async initialize(): Promise<void> {
    const storedSchoolId = sessionStorage.getItem("schoolId");
    if (storedSchoolId) {
      this.currentSchoolId = storedSchoolId;
    }

    // In development, provide mock data
    // In production, this would be set by the PostLoginHandler from the API
    if (this.userSchools.length === 0) {
      this.userSchools = [
        {
          id: "221",
          name: "Fairview Elementary",
          children: [
            { id: "child1", name: "Emma Johnson", grade: "3rd Grade" }
          ]
        },
        {
          id: "291",
          name: "Rock City Elementary",
          children: [
            { id: "child2", name: "Billy Johnson", grade: "1st Grade" }
          ]
        }
      ];
    }
  }

  /**
   * Set user schools from API response (called by PostLoginHandler)
   */
  setUserSchools(schools: UserSchoolDto[]): void {
    this.userSchools = schools;
  }

  /**
   * Set the current school context
   */
  setCurrentSchool(schoolId: string): void {
    const school = this.userSchools.find(s => s.id === schoolId);
    if (!school) {
      throw new Error("School not found in user's schools");
    }
    this.currentSchoolId = schoolId;
    sessionStorage.setItem("schoolId", schoolId);
  }

  /**
   * Get all schools the current user has access to
   */
  getUserSchools(): UserSchoolDto[] {
    return this.userSchools;
  }

  /**
   * Get the currently selected school
   */
  getCurrentSchool(): UserSchoolDto | null {
    if (!this.currentSchoolId) return null;
    return this.userSchools.find(s => s.id === this.currentSchoolId) || null;
  }

  /**
   * Switch to a different school context
   */
  async switchSchool(schoolId: string): Promise<void> {
    const school = this.userSchools.find(s => s.id === schoolId);
    if (!school) {
      throw new Error("User does not have access to this school");
    }

    this.currentSchoolId = schoolId;
    sessionStorage.setItem("schoolId", schoolId);
    
    // could potentially also notify backend about context switch.
    // maybe school context could live in dapr state store?
  }

  /**
   * Check if user has access to multiple schools
   */
  hasMultipleSchools(): boolean {
    return this.userSchools.length > 1;
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
