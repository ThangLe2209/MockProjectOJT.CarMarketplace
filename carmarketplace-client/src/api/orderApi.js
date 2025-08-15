import axiosInstance from "./axiosInstance";

export async function createOrder(orderPayload) {
  const response = await axiosInstance.post("/orders", orderPayload);
  return response.data;
}

export async function getOrdersByBuyerId(buyerId) {
  const response = await axiosInstance.get(`/orders/getByBuyerId/${buyerId}`);
  return response.data.data;
}

export async function getAllOrders(pageSize) {
  const response = await axiosInstance.get("/orders", {
    params: { pageSize },
  });
  return response.data.data;
}

export async function updateOrderStatus(orderId, status) {
  const response = await axiosInstance.put(
    `/orders/${orderId}?status=${status}`
  );
  return response.data;
}
