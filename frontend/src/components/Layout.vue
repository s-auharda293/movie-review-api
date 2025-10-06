<template>
 <div class="flex h-screen">
    <!-- Sidebar / Navbar slot -->
     <div class="w-[10%] md:w-[15%] ">
       <slot name="sidebar"></slot>
     </div>

    <!-- Main content -->
    <div class="flex-1 p-4 overflow-auto">
      <slot name="main"></slot>
    </div>
  </div>
</template>

<script setup></script>
