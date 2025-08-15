import React, { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { Button, Input, Card, Alert, Spin } from "antd";
import { login } from "./authActions";
import { useNavigate } from "react-router-dom";
import { loginUser } from "../../api/authApi";

export default function LoginPage({ isAuthenticated, onLogin }) {
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const handleLogin = async () => {
    setLoading(true); // Show loading spinner
    try {
      const { accessToken, refreshToken } = await loginUser({
        email,
        password,
      });
      dispatch(login({ accessToken, refreshToken }));
      onLogin();
      navigate("/");
    } catch (e) {
      setError("Login failed. Please check your credentials.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (isAuthenticated) {
      navigate("/");
    }
    return () => {};
  }, [isAuthenticated, navigate]);

  // Handle Enter key
  const handleKeyDown = (e) => {
    if (e.key === "Enter") {
      handleLogin();
    }
  };

  return (
    <Card title="Login" style={{ maxWidth: 300, margin: "40px auto" }}>
      {error && (
        <Alert type="error" message={error} style={{ marginBottom: 16 }} />
      )}
      <Input
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        style={{ marginBottom: 16 }}
        onKeyDown={handleKeyDown}
        disabled={loading}
      />
      <Input.Password
        placeholder="Password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
        style={{ marginBottom: 16 }}
        onKeyDown={handleKeyDown}
        disabled={loading}
      />
      <Button type="primary" block onClick={handleLogin} disabled={loading}>
        {loading ? <Spin size="small" /> : "Login"}
      </Button>
      <Button
        block
        onClick={() => navigate("/register")}
        style={{ marginBottom: 8, marginTop: 8 }}
        disabled={loading}
      >
        Register
      </Button>
    </Card>
  );
}
