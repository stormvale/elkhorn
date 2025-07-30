import { RestaurantResponse, restaurantsApi } from "./apiSlice-generated";

// Enhance the generated API with tags for for caching and cache invalidation.
export const restaurantsApiSlice = restaurantsApi.enhanceEndpoints({
  addTagTypes: ["Restaurants", "Meals"],
  endpoints: {
    listRestaurants: {
      providesTags: (result) => result
          ? [
              ...result.map((r: RestaurantResponse) => ({ type: "Restaurants" as const, id: r.id })),
              { type: "Restaurants", id: "LIST" },
            ]
          : [{ type: "Restaurants", id: "LIST" }],
    },

    getRestaurantById: {
      providesTags: (result) => result ? [{ type: "Restaurants", id: result.id }] : []
    },

    registerRestaurant: {
      invalidatesTags: [{ type: "Restaurants", id: "LIST" }]
    },

    deleteRestaurant: {
      invalidatesTags: (_result, _error, arg) => [
        { type: "Restaurants", id: arg },
        { type: "Restaurants", id: "LIST" },
      ]
    },

    createRestaurantMeal: {
      invalidatesTags: (_result, _error, args) => [
        { type: "Restaurants", id: args.restaurantId },
        { type: "Meals", id: "LIST" }
      ]
    },

    deleteRestaurantMeal: {
      invalidatesTags: (_result, _error, args) => [
        { type: "Restaurants", id: args.restaurantId },
        { type: "Meals", id: args.mealId }
      ]
    },
  }
});

// i'm pretty sure we need to re-export the hooks...
export const {
  useRegisterRestaurantMutation,
  useListRestaurantsQuery,
  useGetRestaurantByIdQuery,
  useDeleteRestaurantMutation,
  useCreateRestaurantMealMutation,
  useDeleteRestaurantMealMutation,
} = restaurantsApiSlice;