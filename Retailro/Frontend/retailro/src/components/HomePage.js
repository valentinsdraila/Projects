import { useEffect, useState } from "react";

const HomePage = () => {
  const [username, setUsername] = useState("");
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const productsPerPage = 20;

  useEffect(() => {
    fetch("https://localhost:7007/api/user/me", {
      method: "GET",
      credentials: "include"
    })
      .then(response => {
        if (!response.ok) throw new Error("Failed to fetch username");
        return response.json();
      })
      .then(data => setUsername(data.username))
      .catch(error => console.error(error));
  }, []);

  useEffect(() => {
    fetch("https://localhost:7007/api/products", {
      method: "GET",
      credentials: "include"
    })
      .then(response => {
        if (!response.ok) throw new Error("Failed to fetch products");
        return response.json();
      })
      .then(data => setProducts(data))
      .catch(error => console.error(error));
  }, []);

  const handleLogout = () => {
    fetch("https://localhost:7007/api/auth/logout", {
        method: "POST",
        credentials: "include" // Ensures the cookie is sent with the request
    })
    .then(response => {
        if (response.ok) {
            window.location.href = "/"; // Redirect to login page
        } else {
            console.error("Logout failed");
        }
    })
    .catch(error => console.error("Error:", error));
};

const handleAddToCart = (productId) => {
  fetch(`https://localhost:7007/api/cart/products/${productId}`, {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-Type": "application/json"
    }
  })
  .then(response => {
    if (!response.ok) {
      throw new Error("Failed to add product to cart");
    }
    return response.json();
  })
  .then(data => {
    alert("Product added to cart successfully!");
  })
  .catch(error => console.error("Error:", error));
};

  // Pagination logic
  const indexOfLastProduct = currentPage * productsPerPage;
  const indexOfFirstProduct = indexOfLastProduct - productsPerPage;
  const currentProducts = products.slice(indexOfFirstProduct, indexOfLastProduct);

  return (
    <div className="container mt-5">
      <div className="d-flex justify-content-between align-items-center">
        <h2>Welcome to the Store!</h2>

        {/* Dropdown with Username */}
        <div className="dropdown">
          <button className="btn btn-secondary dropdown-toggle" type="button" id="userDropdown" data-bs-toggle="dropdown" aria-expanded="false">
            {username || "Guest"}
          </button>
          <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="userDropdown">
          <li><a className="dropdown-item" href="/cart">Shopping Cart</a></li>
            <li><a className="dropdown-item" href="#">My Orders</a></li>
            <li><a className="dropdown-item" href="#">Profile</a></li>
            <li><a className="dropdown-item" href="#" onClick={handleLogout}>Logout</a></li>
          </ul>
        </div>
      </div>

      {/* Products List */}
      <div className="row mt-4">
        {currentProducts.map(product => (
          <div key={product.id} className="col-md-3 mb-4">
            <div className="card h-100">
            <img src={`/images/${product.image}`} alt={product.name} className="img-fluid" />
              <div className="card-body">
                <h5 className="card-title">{product.name}</h5>
                <p className="card-text">{product.description}</p>
                <p className="card-text"><strong>${product.unitPrice?.toFixed(2)}</strong></p>
                <button className="btn btn-primary" onClick={() => handleAddToCart(product.id)}>
              Add to Cart
            </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Pagination Controls */}
      {products.length > productsPerPage && (
        <nav>
          <ul className="pagination justify-content-center">
            {Array.from({ length: Math.ceil(products.length / productsPerPage) }, (_, i) => (
              <li key={i} className={`page-item ${currentPage === i + 1 ? "active" : ""}`}>
                <button className="page-link" onClick={() => setCurrentPage(i + 1)}>
                  {i + 1}
                </button>
              </li>
            ))}
          </ul>
        </nav>
      )}
    </div>
  );
};

export default HomePage;
