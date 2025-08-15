import { createStore, applyMiddleware, combineReducers, compose } from "redux";
import { thunk } from "redux-thunk";
import authReducer from "../features/auth/authReducer";
import cartReducer from "../features/order/cartReducer";

const initialState = {
  auth: {
    token: localStorage.getItem("token")
      ? { accessToken: localStorage.getItem("token") }
      : null,
  },
};

const rootReducer = combineReducers({
  auth: authReducer,
  cart: cartReducer,
});

const composeEnhancer = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;

const store = createStore(
  rootReducer,
  initialState,
  composeEnhancer(applyMiddleware(thunk))
);
export default store;
