<script setup>
import logo from "../assets/logo.png";
import { ref } from "vue";
import { loginUser } from "@/services/authService";
import { useRouter } from 'vue-router';
import { useAuth } from "@/composables/useAuth";

const {user} = useAuth();
const router = useRouter();

// form state
const email = ref("");
const password = ref("");
const showPassword = ref(false);

// error messages
const emailError = ref("");
const passwordError = ref("");
const generalError = ref("");

// toggle password visibility
const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

// validate form and submit
const login = async() => {
  let valid = true;

  // reset errors
  emailError.value = "";
  passwordError.value = "";
  generalError.value = "";

  // email validation
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  if (!email.value) {
    emailError.value = "Email is required";
    valid = false;
  } else if (!emailRegex.test(email.value)) {
    emailError.value = "Invalid email address";
    valid = false;
  }

  // password validation
  if (!password.value) {
    passwordError.value = "Password is required";
    valid = false;
  } else if (password.value.length < 8) {
    passwordError.value = "Password must be at least 8 characters";
    valid = false;
  }

  if (!valid) return;


  try {
    // Wait for login to complete
    const loggedInUser = await loginUser(email.value, password.value);

    // Only redirect if login succeeds
    if (!!loggedInUser) {
      router.push("/");
    }
  } catch (err) {
    console.error(err);
    generalError.value = err.value;
  }
};
</script>

<template>
  <div class="flex justify-center items-center min-h-screen p-4">
    <div class="max-w-md w-full bg-white rounded-3xl p-10 shadow-lg hover:shadow-xl transition-shadow duration-300">

      <!-- Header -->
      <div class="text-center mb-8 flex flex-col items-center gap-3">
        <img v-bind:src="logo" alt="logo" class="h-14 w-14" />
        <h1 class="text-4xl font-semibold text-gray-800">Login</h1>
      </div>

      <!-- Form -->
      <form @submit.prevent="login" class="space-y-6">

        <!-- Email -->
        <div>
          <label class="block text-gray-700 font-medium mb-2">Email</label>
          <input
            type="email"
            v-model="email"
            placeholder="Enter your email"
            class="w-full px-4 py-3 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none text-gray-900"
            required
          />
          <p v-if="emailError" class="text-red-500 text-sm mt-1">{{ emailError }}</p>
        </div>

                <!-- Password with eye toggle -->
        <div>
          <label class="block text-gray-700 font-medium mb-2">Password</label>
          <div class="relative">
            <input
              :type="showPassword ? 'text' : 'password'"
              v-model="password"
              placeholder="Enter your password"
              class="w-full px-4 py-3 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none text-gray-900"
            />
            <button
              type="button"
              @click="togglePassword"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-500 hover:text-blue-500 hover:cursor-pointer"
            >
              <i :class="showPassword ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
            </button>
          </div>
          <!-- error outside the relative container -->
          <p v-if="passwordError" class="text-red-500 text-sm mt-1">{{ passwordError }}</p>
        </div>


        <!-- General error -->
        <p v-if="generalError" class="text-red-500 text-center text-sm">{{ generalError }}</p>

        <!-- Submit -->
        <button
          type="submit"
          class="w-full py-3 rounded-full text-white bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 shadow-md transition-all duration-900 hover:cursor-pointer"
        >
          Login
        </button>
      </form>
    </div>
  </div>
</template>
