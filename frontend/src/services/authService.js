import api from "@/api";

export const loginUser = async(email,password)=>{

  try{

    const res = await api.post('/Auth/login',{
      request:{
        email:email,
        password:password
      }
    },
    {withCredentials:true} //stores cookie
  )

  const user = {
  id: res.data.value.id,
  userName: res.data.value.userName,
  email: res.data.value.email
};

sessionStorage.setItem("user", JSON.stringify(user));
sessionStorage.setItem("accessToken",res.data.value.accessToken);

return user;
}catch(error){
  console.error(error);
  throw error;
}
}

export async function logoutUser() {
  try {
    // we add a custom config property to skip the interceptor
    const res = await api.post(
      "/Auth/revoke-refresh-token",
      {},
      {
        withCredentials: true,
        skipAuthInterceptor: true,
      }
    );
    sessionStorage.clear(); //clear accessToken and user information
    // console.info("✅ Logged out and cleared session successfully:", res.data);

    // Use assignment instead of function call
    window.location.href = "/";
  } catch (error) {
    console.error("❌ Error while logging out:", error.response?.data || error);
    throw error;
  }
}


export async function refreshAccessToken() {
  try {
    const res = await api.post('/Auth/generate-access-token', {}, { withCredentials: true });
    const newToken = res.data.value.accessToken;

    if (newToken) {
      sessionStorage.setItem("accessToken", newToken);
      console.info("🔄 Access token refreshed successfully.");
      return newToken;
    } else {
      console.warn("⚠️ No access token found in refresh response.");
      throw new Error("No access token returned from server.");
    }
  } catch (error) {
    console.error("❌ Error while refreshing access token:", error);
    sessionStorage.removeItem("accessToken"); // optional cleanup
    throw error; // rethrow so interceptors or composables can handle re-login
  }
}



