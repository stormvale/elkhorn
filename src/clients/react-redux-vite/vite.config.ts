import react from "@vitejs/plugin-react"
import * as path from "node:path"
import { defineConfig } from "vitest/config"
import packageJson from "./package.json" with { type: "json" }
import { loadEnv } from "vite";

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');

  return {
    plugins: [react()],
    base: "./", // not 100% sure if this is correct...
    
    build: {
      outDir: "dist",
      sourcemap: false, // disable source maps in production
      rollupOptions: {
      output: {
        manualChunks: undefined,  // prevent code splitting
        entryFileNames: 'assets/[name].js',
        chunkFileNames: 'assets/[name].js',
        assetFileNames: 'assets/[name].[ext]'
      }
    }
    },

    server: {
      open: true,
      port: parseInt(env.VITE_PORT) // this comes from the Aspire Host
    },

    hmr: { // hot module reload
      port: parseInt(env.VITE_PORT)
    },

    test: {
      root: import.meta.dirname,
      name: packageJson.name,
      environment: "jsdom",

      typecheck: {
        enabled: true,
        tsconfig: path.join(import.meta.dirname, "tsconfig.json"),
      },

      globals: true,
      watch: false,
      setupFiles: ["./src/setupTests.ts"],
    }
  }
});
