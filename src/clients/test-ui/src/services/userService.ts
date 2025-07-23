export interface UserProfile {
  id: string;
  email: string;
  name: string;
  schools: UserSchool[];
}

export interface UserSchool {
  id: string;
  name: string;
  children?: Child[];
}

export interface Child {
  id: string;
  name: string;
  grade: string;
}

class UserService {
  private baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api';

  async getUserProfile(accessToken: string): Promise<UserProfile> {
    const response = await fetch(`${this.baseUrl}/users/profile`, {
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json'
      }
    });

    if (!response.ok) {
      if (response.status === 404) {
        throw new Error('USER_NOT_FOUND');
      }
      throw new Error('Failed to fetch user profile');
    }

    return response.json();
  }

  async linkSchoolToUser(accessToken: string, schoolId: string): Promise<void> {
    const response = await fetch(`${this.baseUrl}/users/schools`, {
      method: 'POST',
      headers: {
        'Authorization': `Bearer ${accessToken}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({ schoolId })
    });

    if (!response.ok) {
      throw new Error('Failed to link school to user');
    }
  }

  async getAvailableSchools(): Promise<{ id: string; name: string }[]> {
    // For development, return mock data
    // In production, this would fetch from your API
    return new Promise((resolve) => {
      setTimeout(() => {
        resolve([
          { id: "221", name: "Fairview Elementary" },
          { id: "291", name: "Rock City Elementary" },
          { id: "299", name: "Uplands Park Elementary" }
        ]);
      }, 500);
    });
  }
}

export const userService = new UserService();
