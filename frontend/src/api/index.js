import axios from "axios";
import { refreshAccessToken, logoutUser } from "@/services/authService";

const api = axios.create({
  baseURL: "/api",
  withCredentials: true, // include cookies (for refresh token)
});

// ✅ Attach access token to every request
api.interceptors.request.use(config => {
  const token = sessionStorage.getItem("accessToken");
  if (token) config.headers["Authorization"] = `Bearer ${token}`;
  return config;
});

// ✅ Handle 401 responses & refresh token logic
api.interceptors.response.use(
  response => response,
  async error => {
    const originalRequest = error.config;

    // ⛔ If the request explicitly wants to skip interceptor (like logout)
    if (originalRequest?.skipAuthInterceptor) {
      return Promise.reject(error);
    }

    // If response is 401 (unauthorized)
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      // Check if we still have an access token in storage
      const token = sessionStorage.getItem("accessToken");
      if (!token) {
        await logoutUser();
        window.location.href = "/login";
        return Promise.reject(error);
      }

      try {
        // 🔁 Try to get a new access token
        const newToken = await refreshAccessToken();

        // Save it and retry the original request
        sessionStorage.setItem("accessToken", newToken);
        originalRequest.headers["Authorization"] = `Bearer ${newToken}`;
        return api(originalRequest);
      } catch (refreshError) {
        // 🔒 Refresh failed — log out the user
        await logoutUser();
        window.location.href = "/login";
        return Promise.reject(refreshError);
      }
    }

    // For all other errors, just reject
    return Promise.reject(error);
  }
);

export default api;
