const initialState = {
  items: JSON.parse(localStorage.getItem("cartItems") || "[]"),
};

export default function cartReducer(state = initialState, action) {
  switch (action.type) {
    case "ADD_TO_CART":
      // Check if car already in cart
      const existing = state.items.find(
        (item) => item.carId === action.payload.carId
      );
      let updatedItems;
      if (existing) {
        updatedItems = state.items.map((item) =>
          item.carId === action.payload.carId
            ? { ...item, quantity: item.quantity + action.payload.quantity }
            : item
        );
      } else {
        updatedItems = [...state.items, action.payload];
      }
      localStorage.setItem("cartItems", JSON.stringify(updatedItems));
      return { ...state, items: updatedItems };
    case "REMOVE_FROM_CART":
      let updatedNewItems = state.items.filter(
        (item) => item.carId !== action.payload
      );
      localStorage.setItem("cartItems", JSON.stringify(updatedNewItems));
      return { ...state, items: updatedNewItems };
    case "CLEAR_CART":
      localStorage.removeItem("cartItems");
      return { ...state, items: [] };
    default:
      return state;
  }
}
