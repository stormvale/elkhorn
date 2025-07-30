import { apiBase as api } from "./apiSlice-base"
const injectedRtkApi = api.injectEndpoints({
  endpoints: build => ({
    registerUser: build.mutation<RegisterUserApiResponse, RegisterUserApiArg>({
      query: queryArg => ({ url: `/`, method: "POST", body: queryArg }),
    }),
    listUsers: build.query<ListUsersApiResponse, ListUsersApiArg>({
      query: () => ({ url: `/` }),
    }),
    getUserById: build.query<GetUserByIdApiResponse, GetUserByIdApiArg>({
      query: queryArg => ({ url: `/${queryArg}` }),
    }),
    deleteUser: build.mutation<DeleteUserApiResponse, DeleteUserApiArg>({
      query: queryArg => ({ url: `/${queryArg}`, method: "DELETE" }),
    }),
    registerChild: build.mutation<
      RegisterChildApiResponse,
      RegisterChildApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.userId}/kids`,
        method: "POST",
        body: queryArg.registerChildRequest,
      }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as usersApi }
export type RegisterUserApiResponse =
  /** status 201 Created */ RegisterUserResponse
export type RegisterUserApiArg = RegisterUserRequest
export type ListUsersApiResponse = /** status 200 OK */ UserResponse[]
export type ListUsersApiArg = void
export type GetUserByIdApiResponse = /** status 200 OK */ UserResponse
export type GetUserByIdApiArg = string
export type DeleteUserApiResponse = unknown
export type DeleteUserApiArg = string
export type RegisterChildApiResponse = /** status 200 OK */ ChildResponse
export type RegisterChildApiArg = {
  userId: string
  registerChildRequest: RegisterChildRequest
}
export type RegisterUserResponse = {
  userId: string
}
export type RegisterUserRequest = {
  id: string
  name: string
  email: string
}
export type ChildResponse = {
  childId: string
  firstName: string
  lastName: string
  parentId: string
  schoolId: string
  schoolName: string
  grade: string
}
export type UserResponse = {
  id: string
  name: string
  email: string
  children: ChildResponse[]
  schoolIds: string[]
  version: number
}
export type RegisterChildRequest = {
  firstName: string
  lastName: string
  schoolId: string
  schoolName: string
  grade: string
}
export const {
  useRegisterUserMutation,
  useListUsersQuery,
  useGetUserByIdQuery,
  useDeleteUserMutation,
  useRegisterChildMutation,
} = injectedRtkApi
