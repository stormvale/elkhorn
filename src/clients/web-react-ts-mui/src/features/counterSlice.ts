import {createAsyncThunk, createSlice, PayloadAction} from '@reduxjs/toolkit';

interface CounterState {
  value: number;
}

// Define the initial state for the slice
const initialState: CounterState = {
  value: 0,
};

// Create the slice
const counterSlice = createSlice({
  name: "counter",
  initialState,
  reducers: {
    increment: (state, action: PayloadAction<number>) => {
      state.value += action.payload;
    },
    decrement: (state, action: PayloadAction<number>) => {
      state.value -= action.payload;
    },
  },
  
  // async actions go here
  extraReducers: (builder) => {
    builder
      .addCase(incrementAsync.pending, () => {
        console.log("incrementAsync.pending");
      })
      .addCase(incrementAsync.fulfilled, (state, action: PayloadAction<number>) => {
        state.value += action.payload;
      })
  }
});

// async action. whatever gets returned from the async function is available as the payload in the reducer.
export const incrementAsync = createAsyncThunk(
    "counter/incrementAsync",
    async (amount: number) => {
      await new Promise((resolve) => setTimeout(resolve, 1000));
      return amount;
    }
);

// These are the actions that this slice provides
export const { increment, decrement } = counterSlice.actions;

// The reducer function for this slice
export default counterSlice.reducer;


