<script setup>
import { RouterLink } from 'vue-router';

defineProps({
  title:String,
  image:String,
  description:String,
  logo:String
})

const gradients = [
  'from-violet-400 to-indigo-600',
  'from-pink-400 to-purple-600',
  'from-green-400 to-blue-500',
  'from-yellow-400 to-orange-500',
  'from-red-400 to-pink-500'
];

// Pick random gradient for front and back
const frontGradient = gradients[Math.floor(Math.random() * gradients.length)];
const backGradient = gradients[Math.floor(Math.random() * gradients.length)];
</script>

<template>
 <!-- <div class="flex flex-col h-72 w-64 bg-white">
    <h1 class="h-[20%]">Card</h1>
    <div class="h-[80%]">
      <img src="./../assets/logo.png" alt="card-image" class="h-full">
    </div>
 </div> -->

 <!-- <div class="flex flex-col shadow-md rounded-xl overflow-hidden w-72 border border-gray-00">
      <div class="h-[80%] w-full overflow-hidden rounded">
        <img v-bind:src="image" v-bind:alt="title" class="h-full w-full object-cover" />
      </div>
      <button class="text-gray-600 text-2xl font-semibold text-center h-[20%]">
        <RouterLink
        :to="`/${title.toLocaleLowerCase()+'s'}`"
        class=" hover:text-green-300 hover:cursor-pointer w-full h-full"
        >
        {{ title }}
      </RouterLink>
    </button>
  </div> -->


<!-- From Uiverse.io by fthisilak -->
<div class="group relative h-96 w-72 [perspective:1000px]">
  <!-- <div class="absolute duration-1000 w-full h-full [transform-style:preserve-3d] group-hover:[transform:rotateX(180deg)]"> -->
  <div class="absolute duration-1000 w-full h-full [transform-style:preserve-3d]">

    <!-- Front -->
    <div
      :class="`absolute w-full h-full rounded-xl p-6 text-white [backface-visibility:hidden] bg-gradient-to-br ${frontGradient}`"
    >
      <div class="flex flex-col h-full">
        <div class="flex justify-between items-start">
          <div class="text-3xl font-bold">{{ title }}</div>
          <div class="text-5xl">{{logo}}</div>
        </div>
        <div class="mt-4">
          <p class="text-lg text-white-800">{{ description }}</p>
        </div>
        <div class="flex justify-center mt-auto">
          <RouterLink
            :to="`/${title.toLowerCase()}s`"
            class="px-4 py-2 bg-white text-purple-600 rounded-lg font-semibold hover:bg-opacity-90 transition-colors"
          >
            Go to {{ title }}s
          </RouterLink>
        </div>
      </div>
    </div>

    <!-- Back -->
    <!-- <div
      :class="`absolute w-full h-full rounded-xl p-6 text-white [transform:rotateX(180deg)] [backface-visibility:hidden] bg-gradient-to-br ${backGradient}`"
    >
      <div class="flex flex-col h-full">
        <div class="flex-grow">
          <p class="text-lg">Go to {{ title }}s</p>
        </div>
        <div class="flex justify-center mt-auto">
          <RouterLink
            :to="`/${title.toLowerCase()}s`"
            class="px-4 py-2 bg-white text-purple-600 rounded-lg font-semibold hover:bg-opacity-90 transition-colors"
          >
            Go to {{ title }}s
          </RouterLink>
        </div>
      </div>
    </div> -->

  </div>
</div>




</template>
