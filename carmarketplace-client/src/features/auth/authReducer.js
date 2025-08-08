// src/features/auth/authReducer.js
const initialAuthState = { token: null };
export default function authReducer(state = initialAuthState, action) {
  switch (action.type) {
    case "LOGIN":
      return { ...state, token: action.payload };
    case "LOGOUT":
      return { ...state, token: null };
    default:
      return state;
  }
}
