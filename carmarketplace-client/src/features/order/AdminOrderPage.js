import React, { useEffect, useState } from "react";
import {
  Typography,
  List,
  Card,
  Spin,
  Alert,
  Select,
  message,
  Modal,
} from "antd";
import { getAllOrders, updateOrderStatus } from "../../api/orderApi";

export default function AdminOrderPage() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    getAllOrders(5000)
      .then((data) => setOrders(data))
      .catch(() => setError("Could not fetch orders."))
      .finally(() => setLoading(false));
  }, []);

  const handleStatusChange = (orderId, status) => {
    Modal.confirm({
      title: "Confirm Status Change",
      content: `Are you sure you want to change the status to "${status}"?`,
      okText: "Yes",
      cancelText: "No",
      onOk: async () => {
        try {
          await updateOrderStatus(orderId, status);
          message.success("Order status updated!");
          setOrders((orders) =>
            orders.map((order) =>
              order.id === orderId ? { ...order, status } : order
            )
          );
        } catch {
          message.error("Failed to update status.");
        }
      },
    });
  };

  if (loading) return <Spin style={{ margin: 40 }} />;
  if (error)
    return <Alert type="error" message={error} style={{ margin: 40 }} />;

  return (
    <div style={{ padding: 24 }}>
      <Typography.Title level={2}>All Orders (Admin)</Typography.Title>
      <List
        dataSource={[...orders].reverse()}
        renderItem={(order) => (
          <Card style={{ marginBottom: 16 }}>
            <Typography.Text strong>Order #{order.id}</Typography.Text>
            <div>Status: {order.status}</div>
            <div>
              Change Status:{" "}
              <Select
                value={order.status}
                style={{ width: 120 }}
                onChange={(status) => handleStatusChange(order.id, status)}
                options={[
                  { value: "Pending", label: "Pending" },
                  { value: "Completed", label: "Completed" },
                  { value: "Cancelled", label: "Cancelled" },
                ]}
              />
            </div>
            <div>
              Ordered:{" "}
              {order.createdDate
                ? new Date(order.createdDate).toLocaleDateString("en-GB")
                : "N/A"}
            </div>
            <div>Total: ${order.totalPrice}</div>
            <List
              dataSource={order.items}
              renderItem={(item) => (
                <List.Item>
                  Car ID: {item.carId} | Quantity: {item.quantity} | Price: $
                  {item.price}
                </List.Item>
              )}
            />
          </Card>
        )}
      />
    </div>
  );
}
