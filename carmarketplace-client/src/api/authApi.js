import axiosInstance from "./axiosInstance";

export const loginUser = async (credentials) => {
  // Adjust the endpoint and payload as per your backend
  const response = await axiosInstance.post(
    "/authentication/login",
    credentials
  );
  console.log("Login response:", { response });
  if (response.status !== 200) {
    throw new Error("Login failed");
  }
  localStorage.setItem("token", response.data.data.accessToken);
  return response.data.data;
};

export const registerUser = async (credentials) => {
  const response = await axiosInstance.post(
    "/authentication/register",
    credentials
  );
  return response.data.data;
};

export function getClaimFromToken(claimKey) {
  const token = localStorage.getItem("token");
  if (!token) return null;
  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload[claimKey] || null;
  } catch {
    return null;
  }
}
