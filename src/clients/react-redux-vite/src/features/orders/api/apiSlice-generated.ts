import { apiBase as api } from "./apiSlice-base"
const injectedRtkApi = api.injectEndpoints({
  endpoints: build => ({
    post: build.mutation<PostApiResponse, PostApiArg>({
      query: queryArg => ({ url: `/`, method: "POST", body: queryArg }),
    }),
    $get: build.query<$getApiResponse, $getApiArg>({
      query: () => ({ url: `/` }),
    }),
    getById: build.query<GetByIdApiResponse, GetByIdApiArg>({
      query: queryArg => ({ url: `/${queryArg}` }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as ordersApi }
export type PostApiResponse = /** status 201 Created */ string
export type PostApiArg = CreateOrderRequest
export type $getApiResponse = /** status 200 OK */ OrderResponse[]
export type $getApiArg = void
export type GetByIdApiResponse = /** status 200 OK */ OrderResponse
export type GetByIdApiArg = string
export type ContactType = "Contact" | "Parent" | "Manager" | "Principal"
export type Contact = {
  name: string
  email: string
  phone: string | null
  type: ContactType
}
export type CreateOrderRequest = {
  lunchId: string
  contact: Contact
}
export type OrderResponse = {
  orderId: string
}
export const { usePostMutation, use$getQuery, useGetByIdQuery } = injectedRtkApi
