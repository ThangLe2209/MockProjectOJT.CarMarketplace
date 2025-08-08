import React from "react";
import { useSelector } from "react-redux";
import { Navigate } from "react-router-dom";

export default function PrivateRoute({ children }) {
  // For now, just mock authentication
  const isAuthenticated = useSelector((state) => state.auth?.token);
  return isAuthenticated ? children : <Navigate to="/login" />;
}
