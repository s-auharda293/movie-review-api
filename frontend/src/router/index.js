import { createRouter,createWebHistory } from "vue-router"
import HomeView from "@/views/HomeView.vue"
import LoginView from "@/views/LoginView.vue";
import RegisterView from "@/views/RegisterView.vue";
import ReviewView from "@/views/ReviewView.vue";
import MovieView from "@/views/MovieView.vue";
import ActorView from "@/views/ActorView.vue";
import NotFoundView from "@/views/NotFoundView.vue";
import CreateMovieView from "@/components/CreateEditMovieModal.vue";

  const router = createRouter({
    history:createWebHistory(),
    routes:[
      {
        path:'/',
        name:'home',
        component:HomeView,
      }
      ,{
        path:'/login',
        name:'login',
        component:LoginView
      },
      {
        path:'/register',
        name:'register',
        component:RegisterView
      },
      {
        path:'/reviews',
        name:'reviews',
        component:ReviewView
      },
      {
        path:'/movies',
        name:'movies',
        component:MovieView
      },
      {
        path:'/actors',
        name:'actors',
        component:ActorView
      },
      {
        path:'/:catchAll(.*)',
        name:'not-found',
        component:NotFoundView
      },
    ],
  })

  export default router;
