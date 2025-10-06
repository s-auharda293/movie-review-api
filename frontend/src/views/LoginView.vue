<script setup>
import logo from "../assets/logo.png"
</script>

<template>
  <div class="flex flex-col justify-center sm:h-screen p-4">
      <div class="max-w-md w-full mx-auto border border-gray-300 rounded-2xl p-8">
        <div class="text-center mb-12 flex flex-col items-center gap-2">
            <img v-bind:src="logo" alt="logo" class="h-12 w-12" >
            <h1 class="text-3xl">Login</h1>
        </div>

        <form>
          <div class="space-y-6">
            <div>
              <label class="text-slate-900 text-sm font-medium mb-2 block">Email Id</label>
              <input name="email" type="text" class="text-slate-900 bg-white border border-gray-300 w-full text-sm px-4 py-3 rounded-md outline-blue-500" placeholder="Enter email" />
            </div>
            <div>
              <label class="text-slate-900 text-sm font-medium mb-2 block">Password</label>
              <input name="password" type="password" class="text-slate-900 bg-white border border-gray-300 w-full text-sm px-4 py-3 rounded-md outline-blue-500" placeholder="Enter password" />
            </div>
            <div>
              <label class="text-slate-900 text-sm font-medium mb-2 block">Confirm Password</label>
              <input name="cpassword" type="password" class="text-slate-900 bg-white border border-gray-300 w-full text-sm px-4 py-3 rounded-md outline-blue-500" placeholder="Enter confirm password" />
            </div>


          </div>

          <div class="mt-12">
            <button type="button" class="w-full py-3 px-4 text-sm tracking-wider font-medium rounded-md text-white bg-blue-600 hover:bg-blue-700 focus:outline-none cursor-pointer">
              Create an account
            </button>
          </div>
          <p class="text-slate-600 text-sm mt-6 text-center">Already have an account? <a href="javascript:void(0);" class="text-blue-600 font-medium hover:underline ml-1">Login here</a></p>
        </form>
      </div>
    </div>
</template>
