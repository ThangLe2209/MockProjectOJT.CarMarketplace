import React, { useEffect, useRef, useState } from "react";
import {
  Table,
  Button,
  Modal,
  Form,
  Input,
  InputNumber,
  Select,
  message,
  Spin,
} from "antd";
import {
  fetchCarListings,
  createCar,
  updateCar,
  deleteCar,
  restoreCar,
} from "../../api/carApi";

const { Search } = Input;

export default function AdminCarCrudPage() {
  const [cars, setCars] = useState([]);
  const [loading, setLoading] = useState(true);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingCar, setEditingCar] = useState(null);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [total, setTotal] = useState(0);
  const [search, setSearch] = useState("");
  const debounceRef = useRef();

  useEffect(() => {
    // Debounce search
    if (debounceRef.current) clearTimeout(debounceRef.current);
    debounceRef.current = setTimeout(() => {
      fetchCars(page, pageSize, search);
    }, 400); // 400ms debounce
    return () => clearTimeout(debounceRef.current);
    // eslint-disable-next-line
  }, [search, page, pageSize]);

  const fetchCars = async (
    pageNumber = 1,
    pageSizeValue = 10,
    searchTerm = search
  ) => {
    setLoading(true);
    try {
      const { items, pagination } = await fetchCarListings(
        searchTerm,
        pageNumber,
        pageSizeValue,
        "year_desc"
      );
      setCars(items);
      setTotal(pagination?.TotalItemCount || items.length);
    } catch {
      message.error("Failed to fetch cars.");
    } finally {
      setLoading(false);
    }
  };

  const handleEdit = (car) => {
    setEditingCar(car);
    setModalVisible(true);
  };

  const handleDelete = async (carId) => {
    Modal.confirm({
      title: "Confirm Delete",
      content: "Are you sure you want to delete this car?",
      onOk: async () => {
        try {
          await deleteCar(carId);
          message.success("Car deleted!");
          fetchCars(page, pageSize);
        } catch {
          message.error("Failed to delete car.");
        }
      },
    });
  };

  const handleCreate = () => {
    setEditingCar(null);
    setModalVisible(true);
  };

  const handleModalOk = async (values) => {
    try {
      if (editingCar) {
        await updateCar(editingCar.id, values);
        message.success("Car updated!");
      } else {
        await createCar(values);
        message.success("Car created!");
      }
      setModalVisible(false);
      fetchCars(page, pageSize);
    } catch {
      message.error("Failed to save car.");
    }
  };

  const handleTableChange = (pagination) => {
    setPage(pagination.current);
    setPageSize(pagination.pageSize);
    fetchCars(pagination.current, pagination.pageSize, search);
  };

  const handleRestore = async (carId) => {
    try {
      await restoreCar(carId);
      message.success("Car restored!");
      fetchCars(page, pageSize);
    } catch {
      message.error("Failed to restore car.");
    }
  };

  const columns = [
    { title: "ID", dataIndex: "id" },
    { title: "Title", dataIndex: "title" },
    { title: "Status", dataIndex: "status" },
    { title: "Quantity", dataIndex: "quantity" },
    { title: "Price", dataIndex: "price" },
    {
      title: "Actions",
      render: (_, car) => (
        <>
          <Button type="link" onClick={() => handleEdit(car)}>
            Edit
          </Button>
          {car.isDeleted ? (
            <Button type="link" onClick={() => handleRestore(car.id)}>
              Restore
            </Button>
          ) : (
            <Button type="link" danger onClick={() => handleDelete(car.id)}>
              Delete
            </Button>
          )}
        </>
      ),
    },
  ];

  return (
    <div style={{ padding: 24 }}>
      <h2>Admin Car CRUD Page</h2>
      <Search
        placeholder="Search cars"
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        onSearch={(value) => {
          setPage(1);
          fetchCars(1, pageSize, value);
        }}
        allowClear
        enterButton
        style={{ marginBottom: 16, maxWidth: 300 }}
      />
      <Button
        type="primary"
        style={{ marginBottom: 16, marginLeft: 8 }}
        onClick={handleCreate}
      >
        Add Car
      </Button>
      {loading ? (
        <Spin />
      ) : (
        <Table
          dataSource={cars}
          columns={columns}
          rowKey="id"
          pagination={{
            current: page,
            pageSize: pageSize,
            total: total,
            showSizeChanger: true,
            pageSizeOptions: ["5", "10", "20", "50"],
          }}
          onChange={handleTableChange}
        />
      )}
      <Modal
        title={editingCar ? "Edit Car" : "Add Car"}
        open={modalVisible}
        onCancel={() => setModalVisible(false)}
        footer={null}
        destroyOnClose
      >
        <Form
          initialValues={editingCar || { status: "Available", quantity: 1 }}
          onFinish={handleModalOk}
          layout="vertical"
        >
          <Form.Item name="title" label="Title" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item
            name="description"
            label="Description"
            rules={[{ required: true }]}
          >
            <Input.TextArea rows={2} />
          </Form.Item>
          <Form.Item
            name="price"
            label="Price"
            rules={[{ required: true, type: "number", min: 0 }]}
          >
            <InputNumber min={0} />
          </Form.Item>
          <Form.Item name="make" label="Make" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="model" label="Model" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item name="year" label="Year" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item
            name="mileage"
            label="Mileage"
            rules={[{ required: true }]}
          >
            <Input />
          </Form.Item>
          <Form.Item name="color" label="Color" rules={[{ required: true }]}>
            <Input />
          </Form.Item>
          <Form.Item
            name="image"
            label="Image URL"
            rules={[{ required: true }]}
          >
            <Input />
          </Form.Item>
          <Form.Item name="status" label="Status" rules={[{ required: true }]}>
            <Select>
              <Select.Option value="Available">Available</Select.Option>
              <Select.Option value="Reserved">Reserved</Select.Option>
              <Select.Option value="Sold out">Sold out</Select.Option>
            </Select>
          </Form.Item>
          <Form.Item
            name="quantity"
            label="Quantity"
            rules={[{ required: true, type: "number", min: 1 }]}
          >
            <InputNumber min={1} />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit">
              {editingCar ? "Update" : "Create"}
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
