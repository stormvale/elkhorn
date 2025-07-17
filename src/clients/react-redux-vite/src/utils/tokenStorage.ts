import { User } from "../types";

const ACCESS_TOKEN_KEY = 'accessToken';
const USER_KEY = 'user';
const AUTH_STATE_KEY = 'authState';

export const tokenStorage = {
  // Get access token from localStorage
  getAccessToken: (): string | null => {
    try {
      return localStorage.getItem(ACCESS_TOKEN_KEY);
    } catch (error) {
      console.error('Error getting access token from localStorage:', error);
      return null;
    }
  },

  // Set access token in localStorage
  setAccessToken: (token: string): void => {
    try {
      localStorage.setItem(ACCESS_TOKEN_KEY, token);
    } catch (error) {
      console.error('Error setting access token in localStorage:', error);
    }
  },

  // Get user from localStorage
  getUser: (): User | null => {
    try {
      const userJson = localStorage.getItem(USER_KEY);
      return userJson ? JSON.parse(userJson) : null;
    } catch (error) {
      console.error('Error getting user from localStorage:', error);
      return null;
    }
  },

  // Set user in localStorage
  setUser: (user: User): void => {
    try {
      localStorage.setItem(USER_KEY, JSON.stringify(user));
    } catch (error) {
      console.error('Error setting user in localStorage:', error);
    }
  },

  // Get complete auth state
  getAuthState: (): { accessToken: string | null; user: User | null } => {
    return {
      accessToken: tokenStorage.getAccessToken(),
      user: tokenStorage.getUser(),
    };
  },

  // Clear all auth data
  clearAuthData: (): void => {
    try {
      localStorage.removeItem(ACCESS_TOKEN_KEY);
      localStorage.removeItem(USER_KEY);
      localStorage.removeItem(AUTH_STATE_KEY);
    } catch (error) {
      console.error('Error clearing auth data from localStorage:', error);
    }
  },

  // Check if token exists and is not expired (basic check)
  isTokenValid: (): boolean => {
    const token = tokenStorage.getAccessToken();
    if (!token) return false;

    try {
      // Basic JWT expiration check (you might want to use a library like jwt-decode)
      const payload = JSON.parse(atob(token.split('.')[1]));
      const currentTime = Math.floor(Date.now() / 1000);
      return payload.exp > currentTime;
    } catch (error) {
      console.error('Error validating token:', error);
      return false;
    }
  }
};

// Export individual functions for convenience
export const getAccessTokenFromLocalStorage = tokenStorage.getAccessToken;
export const setAccessTokenInLocalStorage = tokenStorage.setAccessToken;
export const getUserFromLocalStorage = tokenStorage.getUser;
export const setUserInLocalStorage = tokenStorage.setUser;
export const clearAuthDataFromLocalStorage = tokenStorage.clearAuthData;