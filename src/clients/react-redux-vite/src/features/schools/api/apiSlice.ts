import { SchoolResponse, schoolsApi } from "./apiSlice-generated";


// Enhance the generated API with tags for for caching and cache invalidation.
export const schoolsApiSlice = schoolsApi.enhanceEndpoints({
  addTagTypes: ["Schools"],
  endpoints: {

    listSchools: {
      providesTags: (result) => result
          ? [
              ...result.map((r: SchoolResponse) => ({ type: "Schools" as const, id: r.id })),
              { type: "Schools", id: "LIST" },
            ]
          : [{ type: "Schools", id: "LIST" }],
    },

    getSchoolById: {
      providesTags: (result) => result ? [{ type: "Schools", id: result.id }] : [],
    },

    registerSchool: {
      invalidatesTags: [{ type: "Schools", id: "LIST" }],
    },

    deleteSchool: {
      invalidatesTags: (_result, _error, arg) => [
        { type: "Schools", id: arg },
        { type: "Schools", id: "LIST" },
      ],
    }
  }
});

// i'm pretty sure we need to re-export the hooks...
export const {
  useRegisterSchoolMutation,
  useListSchoolsQuery,
  useGetSchoolByIdQuery,
  useDeleteSchoolMutation,
  useCreatePacLunchItemMutation,
  useRemovePacLunchItemMutation,
} = schoolsApiSlice;