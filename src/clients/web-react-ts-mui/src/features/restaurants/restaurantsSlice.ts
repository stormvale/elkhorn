import restaurantsApiClient from '../../api/restaurantsApi';
import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';

export interface Modifier {
  name: string;
  priceAdjustment: number;
}

export interface MenuItem {
  id: string;
  name: string;
  price: number;
  availableModifiers: Modifier[];
}

export interface Restaurant {
  id: string;
  name: string;
  contact: { name: string; email: string; phone: string | null; type: string };
  address: { street: string; city: string; postCode: string; state: string };
  menu: MenuItem[];
}

interface State {
  list: Restaurant[];
  selected: Restaurant | null;
  loading: boolean;
}

const initialState: State = {
  list: [],
  selected: null,
  loading: false,
};

export const fetchRestaurants = createAsyncThunk('/', async () => {
  // const res = await axios.get(`${process.env.REACT_APP_API_BASE_URL}/restaurants`);
  const response = await restaurantsApiClient.get("/");
  return response.data;
});

const slice = createSlice({
  name: 'restaurants',
  initialState,

  reducers: {
    setSelectedRestaurant: (state, action: PayloadAction<Restaurant>) => {
      state.selected = action.payload;
    },
    addMeal: (state, action: PayloadAction<MenuItem>) => {
      if (state.selected) state.selected.menu.push(action.payload);
    },
    removeMeal: (state, action: PayloadAction<string>) => {
      if (state.selected)
        state.selected.menu = state.selected.menu.filter((item) => item.id !== action.payload);
    },
  },

  extraReducers: (builder) => {
    builder
      .addCase(fetchRestaurants.pending, (state) => {
        state.loading = true;
      })
      .addCase(fetchRestaurants.fulfilled, (state, action) => {
        state.list = action.payload;
        state.loading = false;
      });
  },
});

export const { setSelectedRestaurant, addMeal, removeMeal } = slice.actions;

export default slice.reducer;