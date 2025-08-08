// src/features/auth/authActions.js
export const login = (token) => ({ type: "LOGIN", payload: token });
export const logout = () => ({ type: "LOGOUT" });
