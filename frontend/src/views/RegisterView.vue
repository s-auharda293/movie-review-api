<script setup>
import logo from "../assets/logo.png";
import { ref } from "vue";
import { registerUser } from "@/services/authService";
import { useRouter } from "vue-router";


const router = useRouter();

// form state
const firstName = ref("");
const lastName = ref("");
const email = ref("");
const password = ref("");
const showPassword = ref(false);

// error messages
const firstNameError = ref("");
const lastNameError = ref("");
const emailError = ref("");
const passwordError = ref("");
const generalError = ref("");

// toggle password visibility
const togglePassword = () => {
  showPassword.value = !showPassword.value;
};

// validate form and submit
const register = async () => {
  let valid = true;

  // reset errors
  firstNameError.value = "";
  lastNameError.value = "";
  emailError.value = "";
  passwordError.value = "";
  generalError.value = "";

  // first name validation
  if (!firstName.value) {
    firstNameError.value = "First name is required";
    valid = false;
  }

  // last name validation
  if (!lastName.value) {
    lastNameError.value = "Last name is required";
    valid = false;
  }

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


  try {
    // Wait for regitser to complete
    const registeredUser = await registerUser(firstName.value, lastName.value,email.value, password.value);

    // Only redirect if regitser succeeds
    if (!!registeredUser) {
      router.push("/");
    }
  } catch (err) {
    console.error(err);
    generalError.value = err.value;
  }

  // Example: general error if registration fails
  // generalError.value = "Email already exists";
};
</script>

<template>
  <div class="flex justify-center items-center min-h-screen p-4">
    <div class="max-w-md w-full bg-white rounded-3xl p-10 shadow-lg hover:shadow-xl transition-shadow duration-300">

      <!-- Header -->
      <div class="text-center mb-8 flex flex-col items-center gap-3">
        <img v-bind:src="logo" alt="logo" class="h-14 w-14" />
        <h1 class="text-4xl font-semibold text-gray-800">Register</h1>
      </div>

      <!-- Form -->
      <form @submit.prevent="register" class="space-y-6">

        <!-- First Name -->
        <div>
          <label class="block text-gray-700 font-medium mb-2">First Name</label>
          <input
            type="text"
            v-model="firstName"
            placeholder="Enter your first name"
            class="w-full px-4 py-3 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none text-gray-900"
          />
          <p v-if="firstNameError" class="text-red-500 text-sm mt-1">{{ firstNameError }}</p>
        </div>

        <!-- Last Name -->
        <div>
          <label class="block text-gray-700 font-medium mb-2">Last Name</label>
          <input
            type="text"
            v-model="lastName"
            placeholder="Enter your last name"
            class="w-full px-4 py-3 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none text-gray-900"
          />
          <p v-if="lastNameError" class="text-red-500 text-sm mt-1">{{ lastNameError }}</p>
        </div>

        <!-- Email -->
        <div>
          <label class="block text-gray-700 font-medium mb-2">Email</label>
          <input
            type="email"
            v-model="email"
            placeholder="Enter your email"
            class="w-full px-4 py-3 rounded-lg border border-gray-300 shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none text-gray-900"
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
              class="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-500 cursor-pointer hover:text-blue-500"
            >
              <i :class="showPassword ? 'pi pi-eye-slash' : 'pi pi-eye'"></i>
            </button>
          </div>
          <p v-if="passwordError" class="text-red-500 text-sm mt-1">{{ passwordError }}</p>
        </div>

        <!-- General error -->
        <p v-if="generalError" class="text-red-500 text-center text-sm">{{ generalError }}</p>

        <!-- Submit -->
        <button
          type="submit"
          class="w-full py-3 rounded-full text-white bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 shadow-md transition-all duration-900 hover:cursor-pointer"
        >
          Register
        </button>

        <p class="text-center text-gray-500 text-sm mt-6">
          Already have an account?
          <a href="javascript:void(0)" class="text-blue-600 font-medium hover:underline ml-1">Login here</a>
        </p>

      </form>
    </div>
  </div>
</template>
