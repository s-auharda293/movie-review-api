import { ref, computed } from "vue";

const user = ref(JSON.parse(sessionStorage.getItem("user")) || null);

export function useAuth() {
  const isLoggedIn = computed(() => !!user.value);

  const setUser = (newUser) => {
    user.value = newUser;
    if (newUser) {
      sessionStorage.setItem("user", JSON.stringify(newUser));
    } else {
      sessionStorage.removeItem("user");
    }
  };

  return {
    user,
    isLoggedIn,
    setUser,
  };
}
