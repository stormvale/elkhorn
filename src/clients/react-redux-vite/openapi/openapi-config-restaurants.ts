import type { ConfigFile } from '@rtk-query/codegen-openapi'

// This approach does require that the Api project is configured
// to generate the OpenAPI schema file on build.

const config: ConfigFile = {
  schemaFile: '../../../services/Restaurants.Api/Restaurants.Api.json',
  apiFile: './emptyApi.ts',
  apiImport: 'emptyApi',
  outputFile: '../src/features/restaurants/api/apiSlice-generated.ts',
  exportName: 'restaurantsApi',
  flattenArg: true,
  hooks: true, // generate use*Query and use*Mutation hooks
  tag: false // doesn't yet handle tags with id's => custom tag management for now.
}

export default config