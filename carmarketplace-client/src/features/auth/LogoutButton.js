import React from "react";
import { useDispatch } from "react-redux";
import { Button } from "antd";
import { logout } from "./authActions";
import { useNavigate } from "react-router-dom";

export default function LogoutButton({ onLogout }) {
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogout = () => {
    dispatch(logout());
    localStorage.removeItem("token"); // Remove token
    navigate("/");
    onLogout(); // Notify App to update UI
  };

  return (
    <Button type="default" onClick={handleLogout} style={{ marginLeft: 8 }}>
      Logout
    </Button>
  );
}
