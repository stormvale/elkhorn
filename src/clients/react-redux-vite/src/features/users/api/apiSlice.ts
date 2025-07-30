import { UserResponse, usersApi } from "./apiSlice-generated";

// Enhance the generated API with tags for for caching and cache invalidation.
export const usersApiSlice = usersApi.enhanceEndpoints({
  addTagTypes: ["Users"],
  endpoints: {
    listUsers: {
      providesTags: (result) => result
          ? [
              ...result.map((r: UserResponse) => ({ type: "Users" as const, id: r.id })),
              { type: "Users", id: "LIST" }
            ]
          : [{ type: "Users", id: "LIST" }]
    },
    getUserById: {
      providesTags: (result) => result ? [{ type: "Users", id: result.id }] : []
    },
    registerUser: {
      invalidatesTags: [{ type: "Users", id: "LIST" }]
    },
    deleteUser: {
      invalidatesTags: (_result, _error, arg) => [
        { type: "Users", id: arg },
        { type: "Users", id: "LIST" }
      ]
    },
    registerChild: {
      invalidatesTags: (_result, _error, args) => [
        { type: "Users", id: args.userId },
        { type: "Users", id: "LIST" }
      ]
    }
  }
});

export const {
  useRegisterUserMutation,
  useListUsersQuery,
  useGetUserByIdQuery,
  useDeleteUserMutation,
  useRegisterChildMutation
} = usersApiSlice;