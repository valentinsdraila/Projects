import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

const AddProductPage = () => {
  const navigate = useNavigate();

  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [stock, setStock] = useState(0);
  const [price, setPrice] = useState(0);
  const [category, setCategory] = useState("");
  const [brand, setBrand] = useState("");
  const [imageFile, setImageFile] = useState(null);
  const [brands, setBrands] = useState([]);
  const [categories, setCategories] = useState([]);
  const [newBrand, setNewBrand] = useState("");
  const [useNewBrand, setUseNewBrand] = useState(false);
  const [newCategory, setNewCategory] = useState("");
  const [useNewCategory, setUseNewCategory] = useState(false);


  useEffect(() => {
    fetch("https://localhost:7007/api/user/me", {
      credentials: "include",
    })
      .then((res) => res.json())
      .then((data) => {
        if (data.role !== "Admin") {
          navigate("/home");
        }
      })
      .catch(() => navigate("/home"));

        fetch("https://localhost:7007/api/products/brands", {
          method:"GET",
          credentials: "include"
        })
    .then((res) => res.json())
    .then((data) =>
        setBrands(data.brands)
        )
    .catch((error) => console.error(error));
  
  fetch("https://localhost:7007/api/products/categories", {
    method:"GET",
    credentials: "include",
  })
    .then((res) => res.json())
        .then((data) =>
        setCategories(data.categories)
        )
    .catch((error) => console.error(error));
  }, [navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();


    if (!name.trim()) return alert("Product name is required.");
    if (!description.trim()) return alert("Description is required.");
    if (stock < 0) return alert("Stock cannot be negative.");
    if (price < 0) return alert("Price cannot be negative.");

    if (useNewBrand) {  
      if (!newBrand.trim()) return alert("Please enter a new brand name.");
    } else {
    if (!brand) return alert("Please select a brand.");
  }

    if (useNewCategory) {
      if (!newCategory.trim()) return alert("Please enter a new category name.");
  } else {
    if (!category) return alert("Please select a category.");
  }
    if (!imageFile) return alert("Please upload a product image.");

    const formData = new FormData();
    formData.append("name", name);
    formData.append("description", description);
    formData.append("stock", stock);
    formData.append("price", price);
    formData.append("category", useNewCategory ? newCategory : category);
    formData.append("brand", useNewBrand ? newBrand : brand);


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
      navigate("/home");
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
          <label className="form-label">Category</label>
          {useNewCategory ? (
            <input
              type="text"
              className="form-control"
              placeholder="Enter new category"
              value={newCategory}
              onChange={(e) => setNewCategory(e.target.value)}
              required
            />
          ) : (
            <select
              className="form-select"
              value={category}
              onChange={(e) => setCategory(e.target.value)}
              required
            >
              <option value="">Select a category</option>
              {categories.map((c) => (
                <option key={c} value={c}>{c}</option>
              ))}
            </select>
          )}
          <div className="form-check form-switch mt-3">
            <input
              className="form-check-input"
              type="checkbox"
              role="switch"
              id="categorySwitch"
              checked={useNewCategory}
              onChange={(e) => setUseNewCategory(e.target.checked)}
            />
            <label className="form-check-label" htmlFor="categorySwitch">
              New Category
            </label>
          </div>
        </div>
        <div className="mb-3">
          <label className="form-label">Brand</label>
          {useNewBrand ? (
            <input
              type="text"
              className="form-control"
              placeholder="Enter new brand"
              value={newBrand}
              onChange={(e) => setNewBrand(e.target.value)}
              required
            />
          ) : (
            <select
              className="form-select"
              value={brand}
              onChange={(e) => setBrand(e.target.value)}
              required
            >
              <option value="">Select a brand</option>
              {brands.map((b) => (
                <option key={b} value={b}>{b}</option>
              ))}
            </select>
          )}
          <div className="form-check form-switch mt-3">
            <input
              className="form-check-input"
              type="checkbox"
              role="switch"
              id="brandSwitch"
              checked={useNewBrand}
              onChange={(e) => setUseNewBrand(e.target.checked)}
            />
            <label className="form-check-label" htmlFor="brandSwitch">
              New Brand
            </label>
          </div>
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
