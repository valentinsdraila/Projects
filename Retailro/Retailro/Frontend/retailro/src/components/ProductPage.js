import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

const ProductPage = () => {
  const { productId } = useParams();
  const [product, setProduct] = useState(null);
  const navigate = useNavigate();
  const [notification, setNotification] = useState(""); 

  useEffect(() => {
    fetch(`https://localhost:7007/api/products/${productId}`, {
      method: "GET",
      credentials: "include"
    })
      .then(response => response.json())
      .then(data => setProduct(data))
      .catch(error => console.error(error));
  }, [productId]);

  const handleAddToCart = () => {
    fetch(`https://localhost:7007/api/cart/products/${product.id}`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
    })
    .then(response => response.json())
    .then(data => {
      if (data.status === "success") {
        setNotification(data.message);
        setTimeout(() => setNotification(""), 3000);
      } else {
        console.error("Error:", data.message);
      }
    })
    .catch(error => {
      console.error("Error:", error);
    });
  };
  

  if (!product) return <div>Loading...</div>;

  return (
    <div className="container mt-5">
      <button className="btn btn-secondary mb-3" onClick={() => navigate(-1)}>Back</button>
      <div className="row">
        <div className="col-md-6">
          <img src={`/images/${product.image}`} alt={product.name} className="img-fluid" />
        </div>
        <div className="col-md-6">
          <h2>{product.name}</h2>
          <p className="lead">{product.description}</p>
          <h4>${product.unitPrice?.toFixed(2)}</h4>
          <button className="btn btn-primary" onClick={handleAddToCart}>Add to Cart</button>
        </div>
      </div>
       {/* Notification Popup */}
       {notification && (
        <div className="notification">
          {notification}
        </div>
      )}
    </div>
  );
};

export default ProductPage;
