import { useEffect, useState } from "react";
import InfiniteScroll from "react-infinite-scroll-component";
import { useNavigate, useSearchParams } from "react-router-dom";
import { fetchCarListings } from "../../api/carApi";
import { Alert, List, Spin } from "antd";
// ...other imports...

export default function CarListPageTest({ isAuthenticated }) {
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const search = searchParams.get("search") || "";
  const pageSize = parseInt(searchParams.get("pageSize") || "10", 10);
  const sort = searchParams.get("sort") || "price_asc";

  useEffect(() => {
    setCars([]);
    setPage(1);
    setHasMore(true);
    fetchMoreCars(1, true);
    // eslint-disable-next-line
  }, [isAuthenticated, search, pageSize, sort]);

  const fetchMoreCars = async (pageToFetch, isFirstLoad = false) => {
    setLoading(true);
    try {
      const data = await fetchCarListings(search, pageToFetch, pageSize, sort);
      setTotal(data.pagination?.TotalItemCount || 0);
      if (isFirstLoad) {
        setCars(data.items || []);
      } else {
        setCars((prev) => [...prev, ...(data.items || [])]);
      }
      if (
        !data.items ||
        data.items.length === 0 ||
        (data.pagination && pageToFetch >= data.pagination.TotalPageCount)
      ) {
        setHasMore(false);
      }
    } catch (e) {
      setError("Could not fetch from API, showing mock data.");
      setHasMore(false);
    } finally {
      setLoading(false);
    }
  };

  const loadMore = () => {
    const nextPage = page + 1;
    setPage(nextPage);
    fetchMoreCars(nextPage);
  };

  return (
    <div style={{ padding: 24 }}>
      <h2>Car Listings</h2>
      {error && (
        <Alert type="warning" message={error} style={{ marginBottom: 16 }} />
      )}
      {/* ...search and sort controls... */}
      <InfiniteScroll
        dataLength={cars.length}
        next={loadMore}
        hasMore={hasMore}
        loader={<Spin style={{ margin: 40 }} />}
        endMessage={
          <div style={{ textAlign: "center", margin: 24 }}>No more cars.</div>
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
            <List.Item>{/* ...your Card code... */}</List.Item>
          )}
        />
      </InfiniteScroll>
    </div>
  );
}
