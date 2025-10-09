import { ref, computed } from "vue";
import {jwtDecode} from "jwt-decode";

const user = ref(JSON.parse(sessionStorage.getItem("user")) || null);

export function useAuth() {

  const isLoggedIn = computed(() => !!user.value);

  const setUser = (token) => {
  sessionStorage.setItem("accessToken",token);
  if (token) {
    try {
      const decodedToken = jwtDecode(token);
      const role = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
      const emailAddress = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
      const userName = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

      user.value = { role,emailAddress,userName };
      if (user.value) {
        sessionStorage.setItem("user", JSON.stringify(user.value));
      }
    } catch (err) {
      console.error("Failed to decode token:", err);
    }
  }
  };

  return {
    user,
    isLoggedIn,
    setUser,
  };
}

