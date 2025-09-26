import { createApp } from 'vue'
import App from './App.vue'
import Toast from 'vue-toastification'
import 'vue-toastification/dist/index.css';
import router from './router/index'

const app = createApp(App)
app.use(router);
app.use(Toast);
app.mount('#app')
