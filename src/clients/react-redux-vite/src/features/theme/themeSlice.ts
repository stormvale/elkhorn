import { createSlice } from "@reduxjs/toolkit"

interface ThemeState {
  mode: string;
}

const initialState: ThemeState = {
  mode: localStorage.getItem('mode') || 'dark'
};

export const themeSlice = createSlice({
  name: "theme",
  initialState,
  reducers: {
    toggleDarkMode: state => {
      if (state.mode === 'light') {
        state.mode = 'dark';
        localStorage.setItem('mode', 'dark');
      } else {
        state.mode = 'light';
        localStorage.setItem('mode', 'light');
      }
    }
  }
})

export const { toggleDarkMode } = themeSlice.actions

export default themeSlice.reducer