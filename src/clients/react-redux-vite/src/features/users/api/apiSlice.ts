import { UserResponse, usersApi } from "./apiSlice-generated";

// Enhance the generated API with tags for for caching and cache invalidation.
export const usersApiSlice = usersApi.enhanceEndpoints({
  addTagTypes: ["Users"],
  endpoints: {
    profile: {
      providesTags: (result) => result ? [{ type: "Users", id: result.id }] : [],
    },

    listUsers: {
      providesTags: (result) => result
          ? [
              ...result.map((r: UserResponse) => ({ type: "Users" as const, id: r.id })),
              { type: "Users", id: "LIST" },
            ]
          : [{ type: "Users", id: "LIST" }],
    },

    getUserById: {
      providesTags: (result) => result ? [{ type: "Users", id: result.id }] : [],
    },

    registerUser: {
      invalidatesTags: [{ type: "Users", id: "LIST" }],
    }
  }
});

// i'm pretty sure we need to re-export the hooks...
export const {
  useRegisterUserMutation,
  useProfileQuery,
  useListUsersQuery,
  useGetUserByIdQuery
} = usersApiSlice;