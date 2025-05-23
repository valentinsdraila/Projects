import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const AddProductPage = () => {
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [stock, setStock] = useState(0);
  const [price, setPrice] = useState(0);
  const [imageFile, setImageFile] = useState(null);

  useEffect(() => {
    fetch("https://localhost:7007/api/user/me", {
      credentials: "include",
    })
      .then((res) => res.json())
      .then((data) => {
        if (data.role !== "Admin") {
          navigate("/unauthorized");
        }
      })
      .catch(() => navigate("/login"));
  }, [navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("name", name);
    formData.append("description", description);
    formData.append("stock", stock);
    formData.append("price", price);
    if (imageFile) {
      formData.append("image", imageFile);
    }

    const response = await fetch("https://localhost:7007/api/products", {
      method: "POST",
      body: formData,
      credentials: "include",
    });

    if (response.ok) {
      alert("Product added successfully!");
      navigate("/admin/products");
    } else {
      alert("Failed to add product.");
    }
  };

  return (
    <div className="container mt-5">
      <h2>Add Product</h2>
      <form onSubmit={handleSubmit} encType="multipart/form-data">
        <div className="mb-3">
          <label className="form-label">Product Name</label>
          <input
            type="text"
            className="form-control"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea
            className="form-control"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Stock</label>
          <input
            type="number"
            className="form-control"
            value={stock}
            onChange={(e) => setStock(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Unit Price ($)</label>
          <input
            type="number"
            className="form-control"
            step="0.01"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Product Image</label>
          <input
            type="file"
            accept="image/*"
            className="form-control"
            onChange={(e) => setImageFile(e.target.files[0])}
          />
        </div>
        <button type="submit" className="btn btn-primary">
          Add Product
        </button>
      </form>
    </div>
  );
};

export default AddProductPage;
