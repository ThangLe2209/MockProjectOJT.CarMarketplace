export const addToCart = (car, quantity) => ({
  type: "ADD_TO_CART",
  payload: {
    carId: car.id,
    title: car.title,
    price: car.price,
    quantity,
    image: car.image,
  },
});

export const removeFromCart = (carId) => ({
  type: "REMOVE_FROM_CART",
  payload: carId,
});

export const clearCart = () => ({
  type: "CLEAR_CART",
});
