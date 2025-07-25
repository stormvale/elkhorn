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
    linkUserToSchool: build.mutation<
      LinkUserToSchoolApiResponse,
      LinkUserToSchoolApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.userId}/schools/${queryArg.schoolId}`,
        method: "POST",
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
export type ProfileApiResponse = /** status 200 OK */ UserProfileResponse
export type ProfileApiArg = void
export type LinkUserToSchoolApiResponse = unknown
export type LinkUserToSchoolApiArg = {
  userId: string
  schoolId: string
}
export type RegisterUserResponse = {
  userId: string
}
export type RegisterUserRequest = {
  id: string
  name: string
  email: string
}
export type UserResponse = {
  id: string
  name: string
  email: string
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
  displayName: string | null
  schools: UserSchoolDto[]
}
export const {
  useRegisterUserMutation,
  useListUsersQuery,
  useGetUserByIdQuery,
  useProfileQuery,
  useLinkUserToSchoolMutation,
} = injectedRtkApi
