import { useEffect, useState } from "react";
import { useNavigate } from 'react-router-dom';

const HomePage = () => {
  const [products, setProducts] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("https://localhost:7007/api/products/newest", {
      method: "GET",
      credentials: "include"
    })
      .then(response => {
        if (!response.ok) throw new Error("Failed to fetch products");
        return response.json();
      })
      .then(data => setProducts(data.newest))
      .catch(error => console.error(error));
  }, []);

  const getStockMessage = (stock) => {
  if (stock > 10) return "In stock";
  if (stock > 0) return "Stock running low";
  return "Out of stock";
};

const getStockClass = (stock) => {
  if (stock > 10) return "text-success";
  if (stock > 0) return "text-warning";
  return "text-danger";
};

  const getStockIcon = (stock) => {
  if (stock > 10) return "bi-check-circle";
  if (stock > 0) return "bi-exclamation-triangle";
  return "bi-x-circle";
};

const renderStars = (rating, clickable = false, onClick = () => {}) => {
  const stars = [];
  for (let i = 1; i <= 5; i++) {
    let starClass = "bi-star";

    if (i <= Math.floor(rating)) {
      starClass = "bi-star-fill"; 
    } else if (i - rating <= 0.5) {
      starClass = "bi-star-half";
    }

    stars.push(
      <i
        key={i}
        className={`bi ${starClass} ${starClass !== "bi-star" ? "text-warning" : "text-secondary"} ${clickable ? "cursor-pointer" : ""}`}
        style={{ cursor: clickable ? "pointer" : "default", fontSize: "1.25rem" }}
        onClick={() => clickable && onClick(i)}
      />
    );
  }
  return stars;
};


  return (
    <div className="container-fluid px-5">
      <div className="row mt-4">
        <h3 className="shadow-sm bg-light rounded px-3 py-2 mb-3 border-start border-5 border-secondary">Just In</h3>
        {products.map(product => (
          <div key={product.id} className="col-12 col-sm-6 col-md-3 mb-4">
              <div
                className="card h-100"
                onClick={() => navigate(`/products/${product.id}`)}
                style={{ cursor: "pointer" }}
              >
                <img
                  src={`https://localhost:7007/images/${product.image}`}
                  alt={product.name}
                  className="img-fluid"
                  style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                />
                <div className="card-body">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">
                    <strong>${product.unitPrice?.toFixed(2)}</strong>
                  </p>
                  <p className={`fw-semibold d-flex align-items-center ${getStockClass(product.quantity)}`}>
                    <i className={`bi ${getStockIcon(product.quantity)} me-2`}></i>
                    {getStockMessage(product.quantity)}
                  </p>
                  <div className="mb-3 d-flex align-items-center">
                    <div>{renderStars(product.rating.averageRating)}</div>
                    <div className="ms-2">{product.rating.averageRating}</div>
                    <div className="ms-2">({product.rating.totalReviews} reviews)</div>
                  </div>
                </div>
              </div>
            </div>
        ))}
      </div>
      <div className="row mt-4">
        <h3 className="shadow-sm bg-light rounded px-3 py-2 mb-3 border-start border-5 border-secondary">Recommended for You</h3>
      </div>
    </div>
  );
};

export default HomePage;
