import { apiBase as api } from "./apiSlice-base"
const injectedRtkApi = api.injectEndpoints({
  endpoints: build => ({
    registerSchool: build.mutation<
      RegisterSchoolApiResponse,
      RegisterSchoolApiArg
    >({
      query: queryArg => ({ url: `/`, method: "POST", body: queryArg }),
    }),
    listSchools: build.query<ListSchoolsApiResponse, ListSchoolsApiArg>({
      query: () => ({ url: `/` }),
    }),
    getSchoolById: build.query<GetSchoolByIdApiResponse, GetSchoolByIdApiArg>({
      query: queryArg => ({ url: `/${queryArg}` }),
    }),
    deleteSchool: build.mutation<DeleteSchoolApiResponse, DeleteSchoolApiArg>({
      query: queryArg => ({ url: `/${queryArg}`, method: "DELETE" }),
    }),
    createPacLunchItem: build.mutation<
      CreatePacLunchItemApiResponse,
      CreatePacLunchItemApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.schoolId}/pac/lunch-items`,
        method: "POST",
        body: queryArg.createLunchItemRequest,
      }),
    }),
    removePacLunchItem: build.mutation<
      RemovePacLunchItemApiResponse,
      RemovePacLunchItemApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.schoolId}/pac/lunch-items`,
        method: "DELETE",
        body: queryArg.removeLunchItemRequest,
      }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as schoolsApi }
export type RegisterSchoolApiResponse =
  /** status 201 Created */ RegisterSchoolResponse
export type RegisterSchoolApiArg = RegisterSchoolRequest
export type ListSchoolsApiResponse = /** status 200 OK */ SchoolResponse[]
export type ListSchoolsApiArg = void
export type GetSchoolByIdApiResponse = /** status 200 OK */ SchoolResponse
export type GetSchoolByIdApiArg = string
export type DeleteSchoolApiResponse = unknown
export type DeleteSchoolApiArg = string
export type CreatePacLunchItemApiResponse = unknown
export type CreatePacLunchItemApiArg = {
  schoolId: string
  createLunchItemRequest: CreateLunchItemRequest
}
export type RemovePacLunchItemApiResponse = unknown
export type RemovePacLunchItemApiArg = {
  schoolId: string
  removeLunchItemRequest: RemoveLunchItemRequest
}
export type RegisterSchoolResponse = {
  schoolId: string
}
export type Address = {
  street: string
  city: string
  postCode: string
  state: string
}
export type ContactType = "Contact" | "Parent" | "Manager" | "Principal"
export type Contact = {
  name: string
  email: string
  phone: string | null
  type: ContactType
}
export type RegisterSchoolRequest = {
  externalId: string
  name: string
  address: Address
  contact: Contact
}
export type LunchItemModifierResponse = {
  name: string
  priceAdjustment: number
}
export type LunchItemResponse = {
  name: string
  price: number
  availableModifiers: LunchItemModifierResponse[]
}
export type PacResponse = {
  id: string
  chairperson: Contact
  lunchItems: LunchItemResponse[]
}
export type SchoolResponse = {
  id: string
  name: string
  externalId: string
  contact: Contact
  address: Address
  pac: PacResponse
  version: number
}
export type CreateLunchItemRequest = {
  name: string
  price: number
}
export type RemoveLunchItemRequest = {
  name: string
}
export const {
  useRegisterSchoolMutation,
  useListSchoolsQuery,
  useGetSchoolByIdQuery,
  useDeleteSchoolMutation,
  useCreatePacLunchItemMutation,
  useRemovePacLunchItemMutation,
} = injectedRtkApi
