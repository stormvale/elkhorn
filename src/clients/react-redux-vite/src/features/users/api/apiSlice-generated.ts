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
    profile: build.query<ProfileApiResponse, ProfileApiArg>({
      query: () => ({ url: `/profile` }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as usersApi }
export type RegisterUserApiResponse = unknown
export type RegisterUserApiArg = RegisterUserRequest
export type ListUsersApiResponse = /** status 200 OK */ UserResponse[]
export type ListUsersApiArg = void
export type GetUserByIdApiResponse = /** status 200 OK */ UserResponse
export type GetUserByIdApiArg = string
export type ProfileApiResponse = /** status 200 OK */ UserProfileResponse
export type ProfileApiArg = void
export type Address = {
  street: string
  city: string
  postCode: string
  state: string
}
export type RegisterUserRequest = {
  name: string
  address: Address
}
export type UserResponse = {
  id: string
  name: string
  address: Address
  version: number
}
export type ChildDto = {
  id: string
  name: string
  grade: string
}
export type UserSchoolDto = {
  id: string
  name: string
  children: ChildDto[]
}
export type UserProfileResponse = {
  id: string
  email: string
  firstName: string | null
  lastName: string | null
  displayName: string | null
  schools: UserSchoolDto[]
  createdAt: string
}
export const {
  useRegisterUserMutation,
  useListUsersQuery,
  useGetUserByIdQuery,
  useProfileQuery,
} = injectedRtkApi
