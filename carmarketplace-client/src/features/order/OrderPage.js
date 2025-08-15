import React, { useEffect, useState } from "react";
import { Typography, List, Card, Spin, Alert } from "antd";
import { getClaimFromToken } from "../../api/authApi";
import { getOrdersByBuyerId } from "../../api/orderApi";

export default function OrderPage() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const buyerId = getClaimFromToken(
      "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
    );
    if (!buyerId) {
      setError("You must be logged in to view your orders.");
      setLoading(false);
      return;
    }
    getOrdersByBuyerId(buyerId)
      .then((data) => setOrders(data))
      .catch(() => setError("Could not fetch orders."))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <Spin style={{ margin: 40 }} />;
  if (error)
    return <Alert type="error" message={error} style={{ margin: 40 }} />;

  return (
    <div style={{ padding: 24 }}>
      <Typography.Title level={2}>Your Orders</Typography.Title>
      <List
        dataSource={[...orders].reverse()}
        renderItem={(order) => (
          <Card style={{ marginBottom: 16 }}>
            <Typography.Text strong>Order #{order.id}</Typography.Text>
            <div>Status: {order.status}</div>
            <div>
              Ordered:{" "}
              {order.createdDate
                ? new Date(order.createdDate).toLocaleString("en-GB")
                : "N/A"}
            </div>
            <div>Total: ${order.totalPrice}</div>
            <List
              dataSource={order.items}
              renderItem={(item) => (
                <List.Item>
                  Car ID: {item.carId} | Quantity: {item.quantity} | Price: $
                  {item.price} | Subtotal: ${item.price * item.quantity}
                </List.Item>
              )}
            />
          </Card>
        )}
      />
    </div>
  );
}
