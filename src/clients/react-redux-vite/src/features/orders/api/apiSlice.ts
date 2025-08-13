import { ordersApi } from "./apiSlice-generated";

// Enhance the generated API with tags for for caching and cache invalidation.
export const ordersApiSlice = ordersApi.enhanceEndpoints({
  addTagTypes: ["Orders"]
});

// i'm pretty sure we need to re-export the hooks...
export const {
  usePostMutation,
  use$getQuery,
  useGetByIdQuery
} = ordersApiSlice;