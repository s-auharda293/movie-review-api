<script setup>
import { logoutUser } from "@/services/authService";
import logo from "../assets/logo.png"
import { useAuth } from "@/composables/useAuth";

const {user,isLoggedIn} = useAuth();

</script>
<template>
  <!-- <aside class="mt-10">
    <div class="flex flex-col items-center">
      <img v-bind:src="logo" alt="logo" class="h-8 w-8 md:h-12 md:w-12 lg:h-20 lg:w-20">
    </div>
    <div class="flex flex-col gap-1 ml-8 mt-7">
      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5  text-gray-500 rounded-full">
          <i class="pi pi-home text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/" class="text-gray-300 no-underline">Home</RouterLink>
      </div>

      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-user text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/login" class="text-gray-300 ">Login</RouterLink>
      </div>

      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-user-plus text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/register" class="text-gray-300 hover:text-gray-300 no-underline ">Register</RouterLink>
      </div>

      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-video text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/movies" class="text-gray-300 hover:text-gray-300 no-underline ">Movies</RouterLink>
      </div>

      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-users text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/actors" class="text-gray-300 hover:text-gray-300 no-underline ">Actors</RouterLink>
      </div>

      <div class="flex items-center gap-5 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-comments text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/reviews" class="text-gray-300 hover:text-gray-300 no-underline ">Reviews</RouterLink>
      </div>
    </div>

  </aside> -->

  <nav class="shadow-md border-r border-gray-200 h-screen min-w-[200px] py-6 overflow-auto w-[20%] bg-white">
      <div class="flex flex-col h-full">
        <div class="flex m-auto">
          <img v-bind:src="logo" alt="logo" class="h-8 w-8 md:h-12 md:w-12 lg:h-18 lg:w-18">
        </div>

        <ul class="space-y-2 my-8 flex-1">

          <!-- Dashboard -->
          <li>
            <RouterLink
              to="/"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-home text-2xl text-gray-500"></i>
              </div>
              <span>Dashboard</span>
            </RouterLink>
          </li>

          <!-- Movies -->
          <li>
            <RouterLink
              to="/movies"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-video text-2xl text-gray-500"></i>
              </div>
              <span>Movies</span>
            </RouterLink>
          </li>

          <!-- Actors -->
          <li>
            <RouterLink
              to="/actors"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-users text-2xl text-gray-500"></i>
              </div>
              <span>Actors</span>
            </RouterLink>
          </li>

          <!-- Reviews -->
          <li>
            <RouterLink
              to="/reviews"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-comments text-2xl text-gray-500"></i>
              </div>
              <span>Reviews</span>
            </RouterLink>
          </li>

          <!-- Login -->
          <li v-if="!isLoggedIn">
            <RouterLink
              to="/login"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-sign-in text-2xl text-gray-500"></i>
              </div>
              <span>Login</span>
            </RouterLink>
          </li>

          <!-- Register -->
          <li v-if="!isLoggedIn">
            <RouterLink
              to="/register"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all"
              active-class="bg-gray-100 text-green-600 border-r-[5px] border-blue-600"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-user-plus text-2xl text-gray-500"></i>
              </div>
              <span>Register</span>
            </RouterLink>
          </li>

          <!-- Logout -->
          <li v-if="isLoggedIn">
            <button
              @click="logoutUser"
              class="text-[15px] font-medium flex items-center gap-3 px-8 py-3.5 transition-all cursor-pointer"
            >
              <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
                <i class="pi pi-sign-out text-2xl text-gray-500"></i>
              </div>
              <span class="text-gray-500">Logout</span>
            </button>
          </li>
        </ul>

       <div class="flex flex-wrap items-center border-t border-gray-300 px-4 py-3.5">
            <!-- User avatar -->
            <img
              v-if="isLoggedIn && user?.userName"
              :src="`https://api.dicebear.com/9.x/avataaars/svg?seed=${user.userName}&eyes=happy`"
              class="w-9 h-9 rounded-full border-white"
            />
            <!-- Default guest avatar -->
            <span
              v-else
              class="pi pi-user"
            ></span>

            <div class="ml-4">
              <p class="text-sm font-medium text-slate-900">{{ user?.userName || 'Guest' }}</p>
              <p class="text-xs text-slate-500 mt-0.5">{{ user?.emailAddress || 'Not Logged In' }}</p>
            </div>
          </div>

      </div>
  </nav>
</template>
