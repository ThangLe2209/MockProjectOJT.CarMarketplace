import React, { useState, useEffect } from "react";
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import CarListPage from "./features/car/CarListPage";
import OrderPage from "./features/order/OrderPage";
import PrivateRoute from "./components/PrivateRoute";
import LoginPage from "./features/auth/LoginPage";
import LogoutButton from "./features/auth/LogoutButton";
import "antd/dist/reset.css";
import { Layout, Menu, Drawer, Button, Grid } from "antd";
import { MenuOutlined } from "@ant-design/icons";
import RegisterPage from "./features/auth/RegisterPage";
// import CarListPageTest from "./features/car/CarListPageTest";

const { useBreakpoint } = Grid;

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [drawerVisible, setDrawerVisible] = useState(false);
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
    ...(isAuthenticated
      ? [
          {
            key: "order",
            label: <Link to="/order">Order</Link>,
          },
        ]
      : []),
    !isAuthenticated
      ? {
          key: "login",
          label: <Link to="/login">Login</Link>,
          style: { float: "right" },
        }
      : {
          key: "logout",
          label: <LogoutButton onLogout={() => setIsAuthenticated(false)} />,
          style: { float: "right" },
        },
  ];

  return (
    <BrowserRouter>
      <Layout>
        <Layout.Header style={{ padding: 0 }}>
          <div
            style={{ display: "flex", alignItems: "center", height: "100%" }}
          >
            {!screens.md && (
              <Button
                type="text"
                icon={<MenuOutlined style={{ color: "#fff", fontSize: 22 }} />}
                onClick={() => setDrawerVisible(true)}
                style={{ marginLeft: 16 }}
              />
            )}
            <div style={{ flex: 1 }}>
              {screens.md ? (
                <Menu
                  theme="dark"
                  mode="horizontal"
                  selectable={false}
                  items={menuItems}
                  style={{ minWidth: 0 }}
                />
              ) : null}
            </div>
          </div>
          <Drawer
            title="Menu"
            placement="left"
            onClose={() => setDrawerVisible(false)}
            open={drawerVisible}
            bodyStyle={{ padding: 0 }}
          >
            <Menu
              mode="vertical"
              selectable={false}
              items={menuItems}
              onClick={() => setDrawerVisible(false)}
            />
          </Drawer>
        </Layout.Header>
        <Layout.Content style={{ minHeight: "80vh" }}>
          <Routes>
            <Route
              path="/"
              element={<CarListPage isAuthenticated={isAuthenticated} />}
            />
            {/* <Route
              path="/test"
              element={<CarListPageTest isAuthenticated={isAuthenticated} />}
            /> */}
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
              element={<LoginPage onLogin={() => setIsAuthenticated(true)} />}
            />
            <Route path="/register" element={<RegisterPage />} />
          </Routes>
        </Layout.Content>
      </Layout>
    </BrowserRouter>
  );
}

export default App;
