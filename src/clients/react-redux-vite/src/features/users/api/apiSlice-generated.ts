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
        url: `/${queryArg.userId}/children`,
        method: "POST",
        body: queryArg.childUpsertRequest,
      }),
    }),
    updateChild: build.mutation<UpdateChildApiResponse, UpdateChildApiArg>({
      query: queryArg => ({
        url: `/${queryArg.userId}/children/${queryArg.childId}`,
        method: "PUT",
        body: queryArg.childUpsertRequest,
      }),
    }),
    removeChild: build.mutation<RemoveChildApiResponse, RemoveChildApiArg>({
      query: queryArg => ({
        url: `/${queryArg.userId}/children/${queryArg.childId}`,
        method: "DELETE",
      }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as usersApi }
export type RegisterUserApiResponse =
  /** status 201 Created */ RegisterUserResponse
export type RegisterUserApiArg = UserUpsertRequest
export type ListUsersApiResponse = /** status 200 OK */ UserResponse[]
export type ListUsersApiArg = void
export type GetUserByIdApiResponse = /** status 200 OK */ UserResponse
export type GetUserByIdApiArg = string
export type DeleteUserApiResponse = unknown
export type DeleteUserApiArg = string
export type RegisterChildApiResponse = /** status 200 OK */ ChildResponse
export type RegisterChildApiArg = {
  userId: string
  childUpsertRequest: ChildUpsertRequest
}
export type UpdateChildApiResponse = unknown
export type UpdateChildApiArg = {
  userId: string
  childId: string
  childUpsertRequest: ChildUpsertRequest
}
export type RemoveChildApiResponse = unknown
export type RemoveChildApiArg = {
  userId: string
  childId: string
}
export type RegisterUserResponse = {
  userId: string
}
export type UserUpsertRequest = {
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
export type ChildUpsertRequest = {
  firstName: string
  lastName: string
  schoolId: string
  schoolName: string
  grade: string
}
export type ProblemDetails = {
  type?: string | null
  title?: string | null
  status?: number | null
  detail?: string | null
  instance?: string | null
}
export const {
  useRegisterUserMutation,
  useListUsersQuery,
  useGetUserByIdQuery,
  useDeleteUserMutation,
  useRegisterChildMutation,
  useUpdateChildMutation,
  useRemoveChildMutation,
} = injectedRtkApi
