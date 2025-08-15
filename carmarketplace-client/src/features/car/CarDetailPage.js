import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { Card, Spin, Alert, Row, Col, Button, Skeleton, Select } from "antd";
import { fetchCarDetail } from "../../api/carApi";
import "./CarDetailPage.css";
import { useDispatch } from "react-redux";
import { addToCart } from "../order/cartActions";

export default function CarDetailPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [car, setCar] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const dispatch = useDispatch();

  // Handle quantity change
  const handleQuantityChange = (value) => {
    setQuantity(value);
  };

  const handleAddToCart = () => {
    dispatch(addToCart(car, quantity));
    navigate("/cart"); // or show a message
  };

  useEffect(() => {
    setLoading(true);
    fetchCarDetail(id)
      .then((data) => setCar(data))
      .catch(() => setError("Could not fetch car details."))
      .finally(() => setLoading(false));
  }, [id]);

  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(`http://localhost:5281/hub/notifications`) // Change port if needed!
      .withAutomaticReconnect()
      .build();

    connection.start().catch(console.error);

    connection.on("CarUpdated", (updatedCarId) => {
      if (parseInt(updatedCarId) === parseInt(id)) {
        // Refetch car details here
        setTimeout(() => {
          // console.log("Socket updated:", updatedCarId);
          setLoading(true);
          fetchCarDetail(id)
            .then((data) => {
              console.log("Fetched car details:", data);
              setCar(data);
            })
            .finally(() => setLoading(false));
        }, 400); // 400ms delay
      }
    });

    return () => {
      connection.stop();
    };
    // eslint-disable-next-line
  }, [id]);

  //   if (loading) return <Spin style={{ margin: 40 }} />;
  if (loading) {
    return (
      <Card
        style={{
          maxWidth: 900,
          margin: "40px auto",
          padding: 24,
          boxShadow: "0 2px 8px rgba(0,0,0,0.08)",
        }}
        bodyStyle={{ padding: 0 }}
      >
        <Row gutter={[32, 16]} align="middle">
          <Col xs={24} md={10}>
            <div className="car-skeleton-wrapper">
              <Skeleton.Image
                style={{ width: "100%", height: 320, borderRadius: 8 }}
                active
              />
            </div>
          </Col>
          <Col xs={24} md={14}>
            <div style={{ padding: 24 }}>
              <Skeleton active paragraph={{ rows: 8 }} />
              <Skeleton.Button active style={{ marginTop: 24, width: 160 }} />
            </div>
          </Col>
        </Row>
      </Card>
    );
  }

  if (error)
    return <Alert type="error" message={error} style={{ margin: 40 }} />;

  if (!car) return null;

  return (
    <Card
      style={{
        maxWidth: 900,
        margin: "40px auto",
        padding: 24,
        boxShadow: "0 2px 8px rgba(0,0,0,0.08)",
      }}
      bodyStyle={{ padding: 0 }}
    >
      <Row gutter={[32, 16]} align="middle">
        <Col xs={24} md={10}>
          <div
            style={{
              width: "100%",
              height: 320,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              background: "#fafafa",
              borderRadius: 8,
              overflow: "hidden",
            }}
          >
            {car.image ? (
              <img
                alt={car.title}
                src={car.image}
                style={{
                  width: "100%",
                  height: "100%",
                  objectFit: "cover",
                  borderRadius: 8,
                }}
              />
            ) : (
              <div
                style={{
                  width: "100%",
                  height: "100%",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  color: "#aaa",
                  fontSize: 24,
                  background: "#eee",
                }}
              >
                No Image
              </div>
            )}
          </div>
        </Col>
        <Col xs={24} md={14}>
          <div style={{ padding: 24 }}>
            <h2 style={{ marginBottom: 16 }}>{car.title}</h2>
            <p>
              <b>Year:</b> {car.year}
            </p>
            <p>
              <b>Make:</b> {car.make}
            </p>
            <p>
              <b>Model:</b> {car.model}
            </p>
            <p>
              <b>Price:</b>{" "}
              <span style={{ color: "#333", fontWeight: "bold" }}>
                ${car.price}
              </span>
            </p>
            <p>
              <b>Mileage:</b> {car.mileage} km
            </p>
            <p>
              <b>Color:</b> {car.color}
            </p>
            <p>
              <b>Description:</b> {car.description}
            </p>
            <p>
              <b>Seller:</b> {car.seller?.userName}
            </p>
            <p>
              <b>Status:</b> {car.status}
            </p>
            {car.status === "Available" && (
              <div style={{ marginBottom: 16 }}>
                <b>Quantity:</b>
                <Select
                  value={quantity}
                  style={{ width: 80 }}
                  onChange={handleQuantityChange}
                >
                  {[...Array(car.quantity).keys()].map((i) => (
                    <Select.Option key={i + 1} value={i + 1}>
                      {i + 1}
                    </Select.Option>
                  ))}
                </Select>
              </div>
            )}
            {car.status === "Available" && (
              <Button
                type="primary"
                size="large"
                style={{ marginTop: 24 }}
                onClick={handleAddToCart}
              >
                Add to Cart
              </Button>
            )}
          </div>
          {/* <Button
                type="primary"
                size="large"
                style={{ marginTop: 24 }}
                onClick={() => navigate("/order")}
              >
                Order This Car
              </Button> */}
        </Col>
      </Row>
    </Card>
  );
}
