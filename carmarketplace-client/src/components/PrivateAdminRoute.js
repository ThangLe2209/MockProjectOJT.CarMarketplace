import React from "react";
import { useSelector } from "react-redux";
import { Navigate } from "react-router-dom";
import { getClaimFromToken } from "../api/authApi";

export default function PrivateAdminRoute({ children }) {
  const isAuthenticated = useSelector((state) => state.auth?.token);
  // Assume your JWT has a claim like "role" or "isAdmin"
  const isAdmin =
    getClaimFromToken(
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ) === "Admin"; // adjust claim name as needed

  return isAuthenticated && isAdmin ? children : <Navigate to="/" />;
}
