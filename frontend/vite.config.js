import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-vue';
import tailwindcss from '@tailwindcss/vite'
import path from "path"
import { fileURLToPath } from "url"

const __filename = fileURLToPath(import.meta.url)
const __dirname = path.dirname(__filename)

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [
      plugin(),
      tailwindcss(),
    ],
    server: {
        port: 3000,
         proxy: {
          '/api': {
            target: 'https://localhost:7289', // replace with your .NET backend URL
            changeOrigin: true,
            secure: false
      },
    },
    },
     resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
})
