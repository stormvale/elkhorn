import { lunchesApi, LunchResponse } from "./apiSlice-generated";

// Enhance the API with cache invalidation
export const lunchesApiSlice = lunchesApi.enhanceEndpoints({
  addTagTypes: ["Lunches"],
  endpoints: {

    listLunches: {
      providesTags: (result) => result
        ? [
            ...result.map((r: LunchResponse) => ({ type: "Lunches" as const, id: r.lunchId })),
            { type: "Lunches", id: "LIST" },
          ]
        : [{ type: "Lunches", id: "LIST" }],
    },

    getLunchById: {
      providesTags: (result) => result ? [{ type: "Lunches", id: result.lunchId }] : [],
    },

    scheduleLunch: {
      invalidatesTags: [{ type: "Lunches", id: "LIST" }],
    },

    cancelLunch: {
      invalidatesTags: (_result, _error, arg) => [
        { type: "Lunches", id: arg },
      ]
    }
  }
});

// Export the hooks
export const {
  useListLunchesQuery,
  useScheduleLunchMutation,
  useGetLunchByIdQuery,
  useCancelLunchMutation,
} = lunchesApiSlice;
