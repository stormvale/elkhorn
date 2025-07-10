import { configureStore } from '@reduxjs/toolkit';
import { apiSlice } from './api/apiSlice';
import userReducer from '@/features/user/userSlice';
import counterReducer from '@/features/counterSlice';
import restaurantsReducer from '@/features/restaurants/restaurantsSlice';

const store = configureStore({
  reducer: {
    [apiSlice.reducerPath]: apiSlice.reducer,
    // Add your reducers here
    counter: counterReducer,
    user: userReducer,
    restaurants: restaurantsReducer
  },
  middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat(apiSlice.middleware),
});

export type RootState = ReturnType<typeof store.getState>;

export type AppDispatch = typeof store.dispatch;

export default store;
