import { getClaimFromToken } from "../../api/authApi";
import { createOrder } from "../../api/orderApi";
import { useSelector, useDispatch } from "react-redux";
import { removeFromCart, clearCart } from "./cartActions";
import { Button, List, message, Modal } from "antd";
import { toast } from "react-toastify";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

export default function CartPage() {
  const [loading, setLoading] = useState(false);
  const [windowWidth, setWindowWidth] = useState(window.innerWidth);
  const items = useSelector((state) => state.cart.items);
  const dispatch = useDispatch();
  const navigate = useNavigate();

  const total = items.reduce(
    (sum, item) => sum + item.price * item.quantity,
    0
  );

  const handleCheckout = () => {
    Modal.confirm({
      title: "Confirm Checkout",
      content: `Are you sure you want to place this order for $${total}?`,
      okText: "Yes",
      cancelText: "No",
      onOk: async () => {
        setLoading(true);
        try {
          const buyerId = getClaimFromToken(
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          );
          if (!buyerId) {
            message.error("You must be logged in to place an order.");
            setLoading(false);
            return;
          }
          const payload = {
            buyerId: parseInt(buyerId),
            status: "Pending",
            items: items.map((item) => ({
              carId: item.carId,
              quantity: item.quantity,
              price: item.price,
            })),
          };
          await createOrder(payload);
          toast.success("Order placed successfully!");
          dispatch(clearCart());
          navigate("/order", { state: { success: true } });
        } catch (error) {
          toast.error(
            error?.response?.data?.message || "Order failed. Please try again."
          );
        } finally {
          setLoading(false);
        }
      },
    });
  };

  useEffect(() => {
    const handleResize = () => setWindowWidth(window.innerWidth);
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  return (
    <div
      style={{
        maxWidth: 600,
        margin: windowWidth > 600 ? "40px auto" : "40px 10px",
      }}
    >
      <h2>Your Cart</h2>
      <List
        dataSource={items}
        renderItem={(item) => (
          <List.Item
            actions={[
              <Button
                danger
                onClick={() => dispatch(removeFromCart(item.carId))}
              >
                Remove
              </Button>,
            ]}
          >
            <List.Item.Meta
              avatar={
                <img src={item.image} alt={item.title} style={{ width: 60 }} />
              }
              title={item.title}
              description={`Quantity: ${item.quantity} | Price: $${item.price}`}
            />
          </List.Item>
        )}
      />
      <h3>Total: ${total}</h3>

      <Button
        type="primary"
        onClick={handleCheckout}
        disabled={items.length === 0 || loading}
        loading={loading}
      >
        Checkout
      </Button>
      <Button
        type="primary"
        style={{ marginLeft: 8 }}
        onClick={() => dispatch(clearCart())}
      >
        Clear Cart
      </Button>
      {/* <Button
        type="default"
        style={{ marginLeft: 8 }}
        onClick={() => window.history.back()}
      >
        Back to previous page
      </Button> */}
    </div>
  );
}
