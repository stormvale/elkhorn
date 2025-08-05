import { apiBase as api } from "./apiSlice-base"
const injectedRtkApi = api.injectEndpoints({
  endpoints: build => ({
    scheduleLunch: build.mutation<
      ScheduleLunchApiResponse,
      ScheduleLunchApiArg
    >({
      query: queryArg => ({ url: `/`, method: "POST", body: queryArg }),
    }),
    listLunches: build.query<ListLunchesApiResponse, ListLunchesApiArg>({
      query: () => ({ url: `/` }),
    }),
    getLunchById: build.query<GetLunchByIdApiResponse, GetLunchByIdApiArg>({
      query: queryArg => ({ url: `/${queryArg}` }),
    }),
    cancelLunch: build.mutation<CancelLunchApiResponse, CancelLunchApiArg>({
      query: queryArg => ({ url: `/${queryArg}`, method: "DELETE" }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as lunchesApi }
export type ScheduleLunchApiResponse = /** status 201 Created */ LunchResponse
export type ScheduleLunchApiArg = ScheduleLunchRequest
export type ListLunchesApiResponse = /** status 200 OK */ LunchResponse[]
export type ListLunchesApiArg = void
export type GetLunchByIdApiResponse = /** status 200 OK */ LunchResponse
export type GetLunchByIdApiArg = string
export type CancelLunchApiResponse = unknown
export type CancelLunchApiArg = string
export type LunchItemModifierResponse = {
  name: string
  priceAdjustment: number
}
export type LunchItemResponse = {
  name: string
  price: number
  availableModifiers: LunchItemModifierResponse[]
}
export type LunchResponse = {
  lunchId: string
  schoolId: string
  restaurantId: string
  date: string
  restaurantLunchItems: LunchItemResponse[]
  pacLunchItems: any[]
}
export type ScheduleLunchRequest = {
  schoolId: string
  restaurantId: string
  date: string
}
export const {
  useScheduleLunchMutation,
  useListLunchesQuery,
  useGetLunchByIdQuery,
  useCancelLunchMutation,
} = injectedRtkApi
