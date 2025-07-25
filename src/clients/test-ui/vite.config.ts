import {defineConfig, loadEnv} from 'vite';
import react from '@vitejs/plugin-react-swc';

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  
  return {
    plugins: [react()],

    server: {
      port: parseInt(env.VITE_PORT) // this comes from the Aspire Host
    },

    css: {
      preprocessorOptions: {
        scss: {
          api: 'modern-compiler', // or "modern"
        },
      },
    },
    
    resolve: {
      alias: {
        '@/app': '/src/app',
        '@/assets': '/src/assets',
        '@/components': '/src/components',
        '@/features': '/src/features',
        '@/hooks': '/src/hooks',
        '@/pages': '/src/pages',
        '@/routes': '/src/routes',
        '@/themes': '/src/themes',
        '@/styles': '/src/styles',
        '@/utils': '/src/utils',
        '@/services': '/src/services',
        '@/config': '/src/config',
        '@/types': '/src/types',
        '@/store': '/src/store',
        '@/layout': '/src/layout',
        '@/data': '/src/data',
        '@/helper': '/src/helper',
        '@/i18n': '/src/i18n',
      },
    },
  }
});
