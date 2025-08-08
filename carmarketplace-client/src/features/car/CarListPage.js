import React, { useEffect, useState } from "react";
import {
  List,
  Card,
  Spin,
  Alert,
  Input,
  Pagination,
  Select,
  Row,
  Col,
  Tooltip,
} from "antd";
import { fetchCarListings } from "../../api/carApi";
import { mockCarListings } from "../../mockCarListings";
import { useSearchParams, useNavigate } from "react-router-dom";
import "./CarListPage.css";

const { Search } = Input;
const { Option } = Select;

export default function CarListPage({ isAuthenticated }) {
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [total, setTotal] = useState(0);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  // Get params from URL
  const search = searchParams.get("search") || "";
  const page = parseInt(searchParams.get("page") || "1", 10);
  const pageSize = parseInt(searchParams.get("pageSize") || "10", 10);
  const sort = searchParams.get("sort") || "price_asc";

  useEffect(() => {
    setLoading(true);
    fetchCarListings(search, page, pageSize, sort)
      .then((data) => {
        setCars(data.items || []);
        // Use pagination info from header if available
        if (data.pagination && data.pagination.TotalItemCount !== undefined) {
          setTotal(data.pagination.TotalItemCount);
        } else {
          setTotal(data.items ? data.items.length : 0);
        }
      })
      .catch((e) => {
        setCars(mockCarListings);
        setTotal(mockCarListings.length);
        setError("Could not fetch from API, showing mock data.");
      })
      .finally(() => setLoading(false));
  }, [isAuthenticated, search, page, pageSize, sort]);

  // Handlers to update URL params
  const handleSearch = (value) => {
    navigate(
      `/?search=${encodeURIComponent(
        value
      )}&page=1&pageSize=${pageSize}&sort=${sort}`
    );
  };

  const handleSortChange = (value) => {
    navigate(
      `/?search=${encodeURIComponent(
        search
      )}&page=1&pageSize=${pageSize}&sort=${value}`
    );
  };

  const handlePageChange = (newPage, newPageSize) => {
    navigate(
      `/?search=${encodeURIComponent(
        search
      )}&page=${newPage}&pageSize=${newPageSize}&sort=${sort}`
    );
  };

  if (loading) return <Spin style={{ margin: 40 }} />;
  return (
    <div style={{ padding: 24 }}>
      <h2>Car Listings</h2>
      {error && (
        <Alert type="warning" message={error} style={{ marginBottom: 16 }} />
      )}
      <Row gutter={[16, 16]} style={{ marginBottom: 16 }}>
        <Col xs={24} sm={12}>
          <Search
            placeholder="Search cars"
            defaultValue={search}
            onSearch={handleSearch}
            allowClear
            enterButton
          />
        </Col>
        <Col xs={24} sm={12}>
          <Select
            value={sort}
            onChange={handleSortChange}
            style={{ width: "100%" }}
          >
            <Option value="price_asc">Price: Low to High</Option>
            <Option value="price_desc">Price: High to Low</Option>
            <Option value="year_desc">Year: Newest</Option>
            <Option value="year_asc">Year: Oldest</Option>
          </Select>
        </Col>
      </Row>
      <List
        grid={{
          gutter: 16,
          xs: 1,
          sm: 2,
          md: 2,
          lg: 3,
          xl: 4,
          xxl: 4,
        }}
        dataSource={cars}
        renderItem={(car) => (
          <List.Item>
            <Card
              title={
                <div className="ellipsis" style={{ width: "100%" }}>
                  {car.title}
                </div>
              }
              cover={
                car.image ? (
                  <img
                    loading="lazy"
                    alt={car.title}
                    src={car.image}
                    style={{ width: "100%", height: 180, objectFit: "cover" }}
                  />
                ) : null
              }
              bodyStyle={{
                height: 250,
                display: "flex",
                flexDirection: "column",
                justifyContent: "space-between",
              }}
            >
              <p className="">
                {car.year} {car.make} {car.model}
              </p>
              <p className="">Price: ${car.price}</p>
              <p className="">Mileage: {car.mileage} km</p>
              <p className="">Color: {car.color}</p>
              <Tooltip title={car.description}>
                <p className="ellipsis">{car.description}</p>
              </Tooltip>
            </Card>
          </List.Item>
        )}
      />
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          marginTop: 24,
          width: "100%",
        }}
      >
        <Pagination
          current={page}
          pageSize={pageSize}
          total={total}
          showSizeChanger
          pageSizeOptions={["1", "2", "5", "10", "15", "20"]}
          onChange={handlePageChange}
          onShowSizeChange={handlePageChange}
        />
      </div>
    </div>
  );
}
