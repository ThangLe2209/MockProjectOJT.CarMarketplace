import axiosInstance from "./axiosInstance";

export async function fetchCarListings(searchTerm, pageNumber, pageSize, sort) {
  const response = await axiosInstance.get("/cars", {
    params: { searchTerm, pageNumber, pageSize, sort },
  });
  // Parse pagination from header
  const paginationHeader = response.headers["x-pagination"];
  // console.log(response);
  let pagination = {};
  if (paginationHeader) {
    try {
      pagination = JSON.parse(paginationHeader);
    } catch {
      pagination = {};
    }
  }
  return {
    items: response.data.data,
    pagination,
  };
}
