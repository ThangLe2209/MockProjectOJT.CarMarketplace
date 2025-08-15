import { getClaimFromToken } from "./authApi";
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

export async function fetchCarDetail(id) {
  const response = await axiosInstance.get(`/Cars/${id}/withseller`);
  // The car data is in response.data.data
  return response.data.data;
}

export async function getAllCars() {
  const response = await axiosInstance.get("/cars?pageSize=100"); // adjust pageSize as needed
  return response.data.data;
}

export async function createCar(carInput) {
  carInput.sellerId = getClaimFromToken(
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
  );
  const response = await axiosInstance.post("/cars", carInput);
  return response.data.data;
}

export async function updateCar(carId, carInput) {
  carInput.sellerId = getClaimFromToken(
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
  );
  await axiosInstance.put(`/cars/${carId}`, carInput);
}

export async function deleteCar(carId) {
  await axiosInstance.delete(`/cars/soft/${carId}`);
}

export async function restoreCar(carId) {
  // Use soft delete endpoint
  await axiosInstance.post(`/cars/restore/${carId}`);
}
