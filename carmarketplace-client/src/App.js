import React, { useState, useEffect } from "react";
import { Routes, Route, Link, useLocation } from "react-router-dom";
import CarListPage from "./features/car/CarListPage";
import OrderPage from "./features/order/OrderPage";
import PrivateRoute from "./components/PrivateRoute";
import LoginPage from "./features/auth/LoginPage";
import LogoutButton from "./features/auth/LogoutButton";
// import "antd/dist/reset.css";
import "antd/dist/antd.css";
import "./App.css";
import { Layout, Menu, Drawer, Button, Grid, Badge } from "antd";
import { useSelector } from "react-redux";
import { MenuOutlined, ShoppingCartOutlined } from "@ant-design/icons";
import RegisterPage from "./features/auth/RegisterPage";
import CarListPageInfiniteScroll from "./features/car/CarListPageInfiniteScroll";
import { getClaimFromToken } from "./api/authApi";
import CarDetailPage from "./features/car/CarDetailPage";
import CartPage from "./features/order/CartPage";
import AdminOrderPage from "./features/order/AdminOrderPage";
import PrivateAdminRoute from "./components/PrivateAdminRoute";
import AdminCarCrudPage from "./features/car/AdminCarCrudPage";

const { useBreakpoint } = Grid;

function App() {
  const isAdmin =
    getClaimFromToken(
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
    ) === "Admin";
  const nameClaim =
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
  const emailClaim =
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
  const userEmail =
    getClaimFromToken(emailClaim) || getClaimFromToken(nameClaim);
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [drawerVisible, setDrawerVisible] = useState(false);

  const cartCount = useSelector((state) => state.cart?.items?.length || 0);
  const location = useLocation();
  const screens = useBreakpoint();

  useEffect(() => {
    const token = localStorage.getItem("token");
    setIsAuthenticated(!!token);
    const handleStorage = () =>
      setIsAuthenticated(!!localStorage.getItem("token"));
    window.addEventListener("storage", handleStorage);
    return () => window.removeEventListener("storage", handleStorage);
  }, []);

  const menuItems = [
    {
      key: "cars",
      label: <Link to="/">Car Listings</Link>,
    },
    {
      key: "cart",
      label: (
        <Link to="/cart">
          <div style={{ position: "relative" }}>
            <span>Cart</span>
            <Badge
              count={cartCount}
              size="small"
              offset={[6, -2]}
              style={{ position: "absolute", top: -18, right: -12 }}
            />
          </div>
        </Link>
      ),
    },
    ...(isAuthenticated
      ? [
          {
            key: "order",
            label: <Link to="/order">Order</Link>,
          },
        ]
      : []),
    ...(isAuthenticated && isAdmin
      ? [
          {
            key: "admin-orders",
            label: <Link to="/admin/orders">Admin Orders</Link>,
          },
          {
            key: "admin-cars",
            label: <Link to="/admin/cars">CRUD Car</Link>,
          },
        ]
      : []),
    !isAuthenticated
      ? {
          key: "login",
          label: <Link to="/login">Login</Link>,
          style: { float: "right" },
        }
      : null,
  ].filter(Boolean); // Remove nulls

  return (
    <Layout>
      <Layout.Header style={{ padding: 0 }}>
        <div style={{ display: "flex", alignItems: "center", height: "100%" }}>
          {/* Hamburger menu for small screens */}
          {!screens.md && (
            <Button
              type="text"
              icon={<MenuOutlined style={{ color: "#fff", fontSize: 22 }} />}
              onClick={() => setDrawerVisible(true)}
              style={{ marginLeft: 16 }}
            />
          )}
          {/* Main menu for medium and up screens */}
          <div style={{ flex: 1 }}>
            {screens.md ? (
              <Menu
                theme="dark"
                mode="horizontal"
                selectable={false}
                items={menuItems}
                style={{ minWidth: 0 }}
                selectedKeys={[
                  location.pathname.startsWith("/admin/orders")
                    ? "admin-orders"
                    : location.pathname.startsWith("/admin/cars")
                    ? "admin-cars"
                    : location.pathname.startsWith("/cart")
                    ? "cart"
                    : location.pathname.startsWith("/order")
                    ? "order"
                    : location.pathname.startsWith("/login")
                    ? "login"
                    : location.pathname === "/"
                    ? "cars"
                    : "",
                ]}
              />
            ) : null}
          </div>
          {/* User info and logout button */}
          {screens.md && isAuthenticated && userEmail && (
            <div
              style={{
                display: "flex",
                alignItems: "center",
                marginRight: 24,
              }}
            >
              <span style={{ color: "#fff", fontWeight: 500, marginRight: 12 }}>
                {userEmail}
              </span>
              <LogoutButton onLogout={() => setIsAuthenticated(false)} />
            </div>
          )}
        </div>
        {/* Drawer for small screens */}
        <Drawer
          title="Menu"
          placement="left"
          onClose={() => setDrawerVisible(false)}
          open={drawerVisible}
          styles={{ body: { padding: 0 } }}
        >
          <Menu
            mode="vertical"
            selectable={false}
            items={[
              ...menuItems,
              isAuthenticated && {
                key: "logout",
                label: (
                  <span>
                    {userEmail}
                    <LogoutButton onLogout={() => setIsAuthenticated(false)} />
                  </span>
                ),
                onClick: () => {
                  localStorage.removeItem("token");
                  setIsAuthenticated(false);
                  setDrawerVisible(false);
                },
              },
            ].filter(Boolean)}
            onClick={() => setDrawerVisible(false)}
            selectedKeys={[
              location.pathname.startsWith("/cart")
                ? "cart"
                : location.pathname.startsWith("/order")
                ? "order"
                : location.pathname.startsWith("/login")
                ? "login"
                : location.pathname === "/"
                ? "cars"
                : "",
            ]}
          />
        </Drawer>
      </Layout.Header>
      <Layout.Content style={{ minHeight: "80vh" }}>
        <Routes>
          <Route
            path="/"
            element={
              screens.md ? (
                <CarListPage isAuthenticated={isAuthenticated} />
              ) : (
                <CarListPageInfiniteScroll isAuthenticated={isAuthenticated} />
              )
            }
          />
          <Route
            path="/test"
            element={
              <CarListPageInfiniteScroll isAuthenticated={isAuthenticated} />
            }
          />
          <Route
            path="/order"
            element={
              <PrivateRoute>
                <OrderPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/login"
            element={
              <LoginPage
                isAuthenticated={isAuthenticated}
                onLogin={() => setIsAuthenticated(true)}
              />
            }
          />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/car/:id" element={<CarDetailPage />} />
          <Route path="/cart" element={<CartPage />} />

          <Route
            path="/admin/orders"
            element={
              <PrivateAdminRoute>
                <AdminOrderPage />
              </PrivateAdminRoute>
            }
          />

          <Route
            path="/admin/cars"
            element={
              <PrivateAdminRoute>
                <AdminCarCrudPage />
              </PrivateAdminRoute>
            }
          />
        </Routes>
      </Layout.Content>
    </Layout>
  );
}

export default App;
