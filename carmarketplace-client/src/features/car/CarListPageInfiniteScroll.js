import {
  Alert,
  Card,
  Col,
  Input,
  List,
  Row,
  Select,
  Skeleton,
  Tooltip,
} from "antd";
import { useEffect, useState } from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { fetchCarListings } from "../../api/carApi";
import { mockCarListings } from "../../mockCarListings";
import "./CarListPage.css";

const { Search } = Input;
const { Option } = Select;

export default function CarListPageInfiniteScroll({ isAuthenticated }) {
  const [cars, setCars] = useState([]);
  // eslint-disable-next-line
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  // const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  // Get params from URL
  const search = searchParams.get("search") || "";
  const pageSize = parseInt(searchParams.get("pageSize") || "2", 10);
  const sort = searchParams.get("sort") || "price_asc";

  // Only reset when search/sort/pageSize changes
  useEffect(() => {
    setCars([]);
    setPage(1);
    setHasMore(true);
    setError(null);
    // setLoading(true);
    fetchCarListings(search, 1, pageSize, sort)
      .then((data) => {
        setCars(data.items || []);
        // setTotal(
        //   data.pagination?.TotalItemCount ||
        //     (data.items ? data.items.length : 0)
        // );
        setHasMore(
          data.pagination
            ? 1 < data.pagination.TotalPageCount
            : (data.items || []).length === pageSize
        );
      })
      .catch((e) => {
        setCars(mockCarListings);
        // setTotal(mockCarListings.length);
        setError("Could not fetch from API, showing mock data.");
        setHasMore(false);
      })
      .finally(() => setLoading(false));
    // eslint-disable-next-line
  }, [isAuthenticated, search, pageSize, sort]);

  // Load more data when scrolling
  const fetchData = () => {
    const nextPage = page + 1;
    setLoading(true);
    fetchCarListings(search, nextPage, pageSize, sort)
      .then((data) => {
        setCars((prev) => [...prev, ...(data.items || [])]);
        setPage(nextPage);
        // setTotal(
        //   data.pagination?.TotalItemCount ||
        //     (data.items ? data.items.length : 0)
        // );
        setHasMore(
          data.pagination
            ? nextPage < data.pagination.TotalPageCount
            : (data.items || []).length === pageSize
        );
      })
      .catch((e) => {
        setError("Could not fetch from API, showing mock data.");
        setHasMore(false);
      })
      .finally(() => setLoading(false));
  };

  // Handlers to update URL params
  const handleSearch = (value) => {
    navigate(
      `/?search=${encodeURIComponent(value)}&pageSize=${pageSize}&sort=${sort}`
    );
  };

  const handleSortChange = (value) => {
    navigate(
      `/?search=${encodeURIComponent(
        search
      )}&pageSize=${pageSize}&sort=${value}`
    );
  };

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
      <InfiniteScroll
        dataLength={cars.length}
        next={fetchData}
        hasMore={hasMore}
        // loader={<Spin style={{ margin: 40 }} />}
        loader={
          <Row gutter={[16, 16]}>
            {[...Array(pageSize)].map((_, idx) => (
              <Col key={idx} xs={24} sm={12} md={12} lg={8} xl={6} xxl={6}>
                <Card style={{ height: 250 }}>
                  <Skeleton active avatar paragraph={{ rows: 4 }} />
                </Card>
              </Col>
            ))}
          </Row>
        }
        endMessage={
          <div style={{ textAlign: "center", margin: 24 }}>
            <b>No more cars.</b>
          </div>
        }
      >
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
                    <Link to={`/car/${car.id}`}>{car.title}</Link>
                  </div>
                }
                cover={
                  car.image ? (
                    <Link to={`/car/${car.id}`}>
                      <img
                        loading="lazy"
                        alt={car.title}
                        src={car.image}
                        style={{
                          width: "100%",
                          height: 180,
                          objectFit: "cover",
                        }}
                        onError={(e) => {
                          e.target.onerror = null;
                          e.target.src =
                            "https://upload.wikimedia.org/wikipedia/commons/1/14/No_Image_Available.jpg"; // Place default-car.png in your public folder
                        }}
                      />
                    </Link>
                  ) : null
                }
                bodyStyle={{
                  height: 250,
                  display: "flex",
                  flexDirection: "column",
                  justifyContent: "space-between",
                }}
              >
                <p>
                  {car.year} {car.make} {car.model}
                </p>
                <p>Price: ${car.price}</p>
                <p>Mileage: {car.mileage} km</p>
                <p>Color: {car.color}</p>
                <Tooltip title={car.description}>
                  <p className="ellipsis">{car.description}</p>
                </Tooltip>
              </Card>
            </List.Item>
          )}
        />
      </InfiniteScroll>
    </div>
  );
}
