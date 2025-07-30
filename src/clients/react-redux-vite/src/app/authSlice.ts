import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { authStorage } from '../utils/authStorage';

export interface UserSchool {
  schoolId: string;
  schoolName: string;
}

export interface UserChild {
  childId: string;
  firstName: string;
  lastName: string;
  grade: string;
  schoolId: string;
}

// this is meant as a utility type to represent an authorized user. if we
// can get this closer to the actual user data structure, we can use that instead.
export interface AuthUser {
  id: string;
  name: string;
  email: string;
  roles: string[];
  schools: UserSchool[];
  children: UserChild[];
}

interface AuthState {
  isAuthenticated: boolean;
  currentUser: AuthUser | null;
  accessToken: string | null;
  currentSchool: UserSchool | null;
}

interface SetCredentialsPayload {
  accessToken: string;
  user: AuthUser;
}

const initializeAuthState = (): AuthState => {
  const { accessToken, user } = authStorage.getAuthState();
  const isTokenValid = accessToken ? authStorage.isTokenValid() : false;

  // initialize from localStorage if we can
  if (isTokenValid && user) {
    return {
      isAuthenticated: true,
      currentUser: user,
      accessToken,
      currentSchool: null
    };
  } else {
    authStorage.clearAuthData();
    return {
      isAuthenticated: false,
      currentUser: null,
      accessToken: null,
      currentSchool: null
    };
  }
};

const initialState: AuthState = initializeAuthState();

export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setCredentials: (state, action: PayloadAction<SetCredentialsPayload>) => {
      const { accessToken, user } = action.payload;

      state.isAuthenticated = true;
      state.currentUser = user;
      state.accessToken = accessToken;

      authStorage.setAccessToken(accessToken);
      authStorage.setUser(user);
      console.log('🔑 User credentials stored in Redux state.');
    },

    clearCredentials: (state) => {
      state.isAuthenticated = false;
      state.currentUser = null;
      state.accessToken = null;
      state.currentSchool = null;
      authStorage.clearAuthData();
    },

    setCurrentSchool: (state, action: PayloadAction<UserSchool>) => {
      state.currentSchool = action.payload;
      sessionStorage.setItem('schoolId', state.currentSchool.schoolId);
    },

    // restoreAuthStateFromLocalStorage: (state) => {
    //   const { accessToken, user } = authStorage.getAuthState();
    //   const isTokenValid = accessToken ? authStorage.isTokenValid() : false;

    //   if (isTokenValid && user) {
    //     state.isAuthenticated = true;
    //     state.currentUser = user;
    //     state.accessToken = accessToken;
    //   } else {
    //     state.isAuthenticated = false;
    //     state.currentUser = null;
    //     state.accessToken = null;
    //     state.currentSchool = null;
    //     authStorage.clearAuthData();
    //   }
    // }
  }
});

export const {
  setCredentials,
  clearCredentials,
  //restoreAuthStateFromLocalStorage,
  //setSchoolContext,
  setCurrentSchool 
} = authSlice.actions;

export default authSlice.reducer;