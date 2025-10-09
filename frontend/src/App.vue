<script setup>
import { RouterView } from 'vue-router';
import Navbar from "./components/Navbar.vue";
import Layout from "./components/Layout.vue";
</script>

<template>
  <Layout>
    <!-- fills the sidebar slot in Layout with <Navbar /> -->
    <template v-slot:sidebar>
      <Navbar />
    </template>

    <!-- fills the main slot in Layout with <RouterView />. -->
    <template v-slot:main>
      <RouterView />
    </template>
  </Layout>
</template>

