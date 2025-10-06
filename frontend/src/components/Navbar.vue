<script setup>
import logo from "../assets/logo.png"
</script>
<template>
  <!-- <aside class="mt-10">
    <div class="flex flex-col items-center">
      <img v-bind:src="logo" alt="logo" class="h-8 w-8 md:h-12 md:w-12 lg:h-20 lg:w-20">
    </div>
    <div class="flex flex-col gap-1 ml-8 mt-7">
      <div class="flex items-center gap-2 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5  text-gray-500 rounded-full">
          <i class="pi pi-home text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/" class="text-gray-300 no-underline">Home</RouterLink>
      </div>

      <div class="flex items-center gap-2 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-user text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/login" class="text-gray-300 ">Login</RouterLink>
      </div>

      <div class="flex items-center gap-2 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-user-plus text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/register" class="text-gray-300 hover:text-gray-300 no-underline ">Register</RouterLink>
      </div>

      <div class="flex items-center gap-2 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-video text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/movies" class="text-gray-300 hover:text-gray-300 no-underline ">Movies</RouterLink>
      </div>

      <div class="flex items-center gap-2 p-1 rounded">
        <div class="flex items-center justify-center w-8 h-8 border p-5 text-gray-500 rounded-full">
          <i class="pi pi-users text-2xl text-gray-500"></i>
        </div>
        <RouterLink to="/actors" class="text-gray-300 hover:text-gray-300 no-underline ">Actors</RouterLink>
      </div>

      <div class="flex items-center gap-2 p-1 rounded">
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
          <li>
            <RouterLink
  to="/"
  class="text-[15px] font-medium flex items-center gap-1 px-8 py-3.5 transition-all"
  active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
>
                <div class="flex items-center justify-center w-3 h-3 border p-4  text-gray-500 rounded-full">
                          <i class="pi pi-home text-2xl text-gray-500"></i>
                </div>
                <path
                  d="M19.56 23.253H4.44a4.051 4.051 0 0 1-4.05-4.05v-9.115c0-1.317.648-2.56 1.728-3.315l7.56-5.292a4.062 4.062 0 0 1 4.644 0l7.56 5.292a4.056 4.056 0 0 1 1.728 3.315v9.115a4.051 4.051 0 0 1-4.05 4.05zM12 2.366a2.45 2.45 0 0 0-1.393.443l-7.56 5.292a2.433 2.433 0 0 0-1.037 1.987v9.115c0 1.34 1.09 2.43 2.43 2.43h15.12c1.34 0 2.43-1.09 2.43-2.43v-9.115c0-.788-.389-1.533-1.037-1.987l-7.56-5.292A2.438 2.438 0 0 0 12 2.377z"
                  data-original="#000000"></path>
                <path
                  d="M16.32 23.253H7.68a.816.816 0 0 1-.81-.81v-5.4c0-2.83 2.3-5.13 5.13-5.13s5.13 2.3 5.13 5.13v5.4c0 .443-.367.81-.81.81zm-7.83-1.62h7.02v-4.59c0-1.933-1.577-3.51-3.51-3.51s-3.51 1.577-3.51 3.51z"
                  data-original="#000000"></path>
              <span>Dashboard</span>
            </RouterLink>
          </li>

           <li>
            <RouterLink
                to="/movies"
                class="text-[15px] font-medium flex items-center gap-1 px-8 py-3.5 transition-all"
                active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
              >
                <div class="flex items-center justify-center w-3 h-3 border p-4  text-gray-500 rounded-full">
                          <i class="pi pi-video text-2xl text-gray-500"></i>
                </div>
                <path
                  d="M19.56 23.253H4.44a4.051 4.051 0 0 1-4.05-4.05v-9.115c0-1.317.648-2.56 1.728-3.315l7.56-5.292a4.062 4.062 0 0 1 4.644 0l7.56 5.292a4.056 4.056 0 0 1 1.728 3.315v9.115a4.051 4.051 0 0 1-4.05 4.05zM12 2.366a2.45 2.45 0 0 0-1.393.443l-7.56 5.292a2.433 2.433 0 0 0-1.037 1.987v9.115c0 1.34 1.09 2.43 2.43 2.43h15.12c1.34 0 2.43-1.09 2.43-2.43v-9.115c0-.788-.389-1.533-1.037-1.987l-7.56-5.292A2.438 2.438 0 0 0 12 2.377z"
                  data-original="#000000"></path>
                <path
                  d="M16.32 23.253H7.68a.816.816 0 0 1-.81-.81v-5.4c0-2.83 2.3-5.13 5.13-5.13s5.13 2.3 5.13 5.13v5.4c0 .443-.367.81-.81.81zm-7.83-1.62h7.02v-4.59c0-1.933-1.577-3.51-3.51-3.51s-3.51 1.577-3.51 3.51z"
                  data-original="#000000"></path>
              <span>Movies</span>
            </RouterLink>
          </li>
           <li>
            <RouterLink
                to="/actors"
                class="text-[15px] font-medium flex items-center gap-1 px-8 py-3.5 transition-all"
                active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
              >
                <div class="flex items-center justify-center w-3 h-3 border p-4  text-gray-500 rounded-full">
                          <i class="pi pi-users text-2xl text-gray-500"></i>
                </div>
                <path
                  d="M19.56 23.253H4.44a4.051 4.051 0 0 1-4.05-4.05v-9.115c0-1.317.648-2.56 1.728-3.315l7.56-5.292a4.062 4.062 0 0 1 4.644 0l7.56 5.292a4.056 4.056 0 0 1 1.728 3.315v9.115a4.051 4.051 0 0 1-4.05 4.05zM12 2.366a2.45 2.45 0 0 0-1.393.443l-7.56 5.292a2.433 2.433 0 0 0-1.037 1.987v9.115c0 1.34 1.09 2.43 2.43 2.43h15.12c1.34 0 2.43-1.09 2.43-2.43v-9.115c0-.788-.389-1.533-1.037-1.987l-7.56-5.292A2.438 2.438 0 0 0 12 2.377z"
                  data-original="#000000"></path>
                <path
                  d="M16.32 23.253H7.68a.816.816 0 0 1-.81-.81v-5.4c0-2.83 2.3-5.13 5.13-5.13s5.13 2.3 5.13 5.13v5.4c0 .443-.367.81-.81.81zm-7.83-1.62h7.02v-4.59c0-1.933-1.577-3.51-3.51-3.51s-3.51 1.577-3.51 3.51z"
                  data-original="#000000"></path>
              <span>Actors</span>
            </RouterLink>
          </li>
           <li>
            <RouterLink
                to="/reviews"
                class="text-[15px] font-medium flex items-center gap-1 px-8 py-3.5 transition-all"
                active-class="bg-gray-100 text-blue-600 border-r-[5px] border-blue-600"
              >
                <div class="flex items-center justify-center w-3 h-3 border p-4  text-gray-500 rounded-full">
                          <i class="pi pi-comments text-2xl text-gray-500"></i>
                </div>
                <path
                  d="M19.56 23.253H4.44a4.051 4.051 0 0 1-4.05-4.05v-9.115c0-1.317.648-2.56 1.728-3.315l7.56-5.292a4.062 4.062 0 0 1 4.644 0l7.56 5.292a4.056 4.056 0 0 1 1.728 3.315v9.115a4.051 4.051 0 0 1-4.05 4.05zM12 2.366a2.45 2.45 0 0 0-1.393.443l-7.56 5.292a2.433 2.433 0 0 0-1.037 1.987v9.115c0 1.34 1.09 2.43 2.43 2.43h15.12c1.34 0 2.43-1.09 2.43-2.43v-9.115c0-.788-.389-1.533-1.037-1.987l-7.56-5.292A2.438 2.438 0 0 0 12 2.377z"
                  data-original="#000000"></path>
                <path
                  d="M16.32 23.253H7.68a.816.816 0 0 1-.81-.81v-5.4c0-2.83 2.3-5.13 5.13-5.13s5.13 2.3 5.13 5.13v5.4c0 .443-.367.81-.81.81zm-7.83-1.62h7.02v-4.59c0-1.933-1.577-3.51-3.51-3.51s-3.51 1.577-3.51 3.51z"
                  data-original="#000000"></path>
              <span>Reviews</span>
            </RouterLink>
          </li>
        </ul>

        <div class="flex flex-wrap items-center cursor-pointer border-t border-gray-300 px-4 py-3.5">
          <img src='https://readymadeui.com/profile.webp' class="w-9 h-9 rounded-full border-white" />
          <div class="ml-4">
            <p class="text-sm font-medium text-slate-900">John Doe</p>
            <p class="text-xs text-slate-500 mt-0.5">Active free account</p>
          </div>
        </div>
      </div>
  </nav>
</template>
