<script setup>
import Card from "@/components/Card.vue";
import actor from "@/assets/actor.jpg";
import movie from "@/assets/movie.jpg";
import review from "@/assets/review.jpg";

</script>

<template>
  <h1 class="text-gray-500 text-xl mb-10 mt-2">Home</h1>
  <div class="grid grid-cols-3 gap-x-48 h-88 m-l-16 w-[70%]">
    <Card title="Movie" v-bind:image="movie" description="Explore movies, trailers, and reviews."
      logo="🎥"/>

    <Card title="Actor" v-bind:image="actor"  description="Discover famous actors and their works."
      logo="🎭" />

    <Card title="Review" v-bind:image="review" description="Read and write reviews for movies."
      logo="💬"/>

    <!-- <Card/> -->
  </div>
</template>
