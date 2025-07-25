import type { Action, ThunkAction } from "@reduxjs/toolkit"
import { combineSlices, configureStore } from "@reduxjs/toolkit"
import { setupListeners } from "@reduxjs/toolkit/query"
import { quotesApiSlice } from "../features/quotes/quotesApiSlice"
import { restaurantsApiSlice } from "../features/restaurants/api/apiSlice"
import { themeSlice } from "../theme/themeSlice"
import { errorMiddleware } from "../middleware/errorMiddleware"
import { notificationSlice } from "../features/notifications/notificationSlice"
import { authSlice } from "./authSlice"
import { usersApiSlice } from "../features/users/api/apiSlice"
import { schoolsApiSlice } from "../features/schools/api/apiSlice"

// `combineSlices` automatically combines the reducers using the `reducerPath`
const rootReducer = combineSlices(
  quotesApiSlice,
  restaurantsApiSlice,
  schoolsApiSlice,
  usersApiSlice,
  themeSlice,
  authSlice,
  notificationSlice
)

// Infer the `RootState` type from the root reducer
export type RootState = ReturnType<typeof rootReducer>

// The store setup is wrapped in `makeStore` to allow reuse
// when setting up tests that need the same store config
export const makeStore = (preloadedState?: Partial<RootState>) => {
  const store = configureStore({
    reducer: rootReducer,
    
    // Adding the api middleware enables caching, invalidation, polling and other useful features of `rtk-query`.
    middleware: getDefaultMiddleware => {
      return getDefaultMiddleware()
        .concat(quotesApiSlice.middleware)
        .concat(restaurantsApiSlice.middleware)
        .concat(schoolsApiSlice.middleware)
        .concat(usersApiSlice.middleware)
        .concat(errorMiddleware)
    },
    preloadedState,
  })
  // configure listeners using the provided defaults
  // optional, but required for `refetchOnFocus`/`refetchOnReconnect` behaviors
  setupListeners(store.dispatch)
  return store
}

export const store = makeStore()

// Infer the type of `store`
export type AppStore = typeof store

// Infer the `AppDispatch` type from the store itself
export type AppDispatch = AppStore["dispatch"]

export type AppThunk<ThunkReturnType = void> = ThunkAction<
  ThunkReturnType,
  RootState,
  unknown,
  Action
>
