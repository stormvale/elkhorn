import { apiBase as api } from "./apiSlice-base"
const injectedRtkApi = api.injectEndpoints({
  endpoints: build => ({
    registerRestaurant: build.mutation<
      RegisterRestaurantApiResponse,
      RegisterRestaurantApiArg
    >({
      query: queryArg => ({ url: `/`, method: "POST", body: queryArg }),
    }),
    listRestaurants: build.query<
      ListRestaurantsApiResponse,
      ListRestaurantsApiArg
    >({
      query: () => ({ url: `/` }),
    }),
    getRestaurantById: build.query<
      GetRestaurantByIdApiResponse,
      GetRestaurantByIdApiArg
    >({
      query: queryArg => ({ url: `/${queryArg}` }),
    }),
    deleteRestaurant: build.mutation<
      DeleteRestaurantApiResponse,
      DeleteRestaurantApiArg
    >({
      query: queryArg => ({ url: `/${queryArg}`, method: "DELETE" }),
    }),
    createRestaurantMeal: build.mutation<
      CreateRestaurantMealApiResponse,
      CreateRestaurantMealApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.restaurantId}/meals`,
        method: "POST",
        body: queryArg.createMealRequest,
      }),
    }),
    deleteRestaurantMeal: build.mutation<
      DeleteRestaurantMealApiResponse,
      DeleteRestaurantMealApiArg
    >({
      query: queryArg => ({
        url: `/${queryArg.restaurantId}/meals/${queryArg.mealId}`,
        method: "DELETE",
      }),
    }),
  }),
  overrideExisting: false,
})
export { injectedRtkApi as restaurantsApi }
export type RegisterRestaurantApiResponse =
  /** status 201 Created */ RegisterRestaurantResponse
export type RegisterRestaurantApiArg = RegisterRestaurantRequest
export type ListRestaurantsApiResponse =
  /** status 200 OK */ RestaurantResponse[]
export type ListRestaurantsApiArg = void
export type GetRestaurantByIdApiResponse =
  /** status 200 OK */ RestaurantResponse
export type GetRestaurantByIdApiArg = string
export type DeleteRestaurantApiResponse = unknown
export type DeleteRestaurantApiArg = string
export type CreateRestaurantMealApiResponse =
  /** status 200 OK */ CreateMealResponse
export type CreateRestaurantMealApiArg = {
  restaurantId: string
  createMealRequest: CreateMealRequest
}
export type DeleteRestaurantMealApiResponse = unknown
export type DeleteRestaurantMealApiArg = {
  restaurantId: string
  mealId: string
}
export type RegisterRestaurantResponse = {
  restaurantId: string
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
export type RegisterRestaurantRequest = {
  name: string
  address: Address
  contact: Contact
}
export type RestaurantMealModifierResponse = {
  name: string
  priceAdjustment: number
}
export type RestaurantMealResponse = {
  id: string
  name: string
  price: number
  availableModifiers: RestaurantMealModifierResponse[]
}
export type RestaurantResponse = {
  id: string
  name: string
  contact: Contact
  address: Address
  menu: RestaurantMealResponse[]
}
export type ProblemDetails = {
  type?: string | null
  title?: string | null
  status?: number | null
  detail?: string | null
  instance?: string | null
}
export type CreateMealResponse = {
  mealId: string
  restaurantId: string
}
export type MealModifierDto = {
  name: string
  priceAdjustment: number
}
export type CreateMealRequest = {
  name: string
  price: number
  modifiers: MealModifierDto[]
}
export const {
  useRegisterRestaurantMutation,
  useListRestaurantsQuery,
  useGetRestaurantByIdQuery,
  useDeleteRestaurantMutation,
  useCreateRestaurantMealMutation,
  useDeleteRestaurantMealMutation,
} = injectedRtkApi
