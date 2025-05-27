import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

const ProductPage = () => {
  const { productId } = useParams();
  const [product, setProduct] = useState(null);
  const navigate = useNavigate();
  const [notification, setNotification] = useState("");
  const [showModal, setShowModal] = useState(false);
  const [reviewRating, setReviewRating] = useState(0);
  const [reviewComment, setReviewComment] = useState("");

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

  const handleReviewSubmit = () => {
    fetch(`https://localhost:7007/api/review/product/${productId}`, {
      method: "POST",
      credentials: "include",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        rating: reviewRating,
        comment: reviewComment
      })
    })
      .then(response => {
        if (response.ok) {
          setShowModal(false);
          setReviewRating(0);
          setReviewComment("");
          // Re-fetch product to update reviews
          return fetch(`https://localhost:7007/api/products/${productId}`, {
            method: "GET",
            credentials: "include"
          });
        } else {
          throw new Error("Failed to post review");
        }
      })
      .then(response => response.json())
      .then(data => setProduct(data))
      .catch(error => {
        console.error("Error:", error);
        alert("Error submitting review");
      });
  };

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

  const renderStars = (rating, clickable = false, onClick = () => {}) => {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      stars.push(
        <i
          key={i}
          className={`bi ${i <= rating ? "bi-star-fill text-warning" : "bi-star text-secondary"} ${clickable ? "cursor-pointer" : ""}`}
          style={{ cursor: clickable ? "pointer" : "default", fontSize: "1.25rem" }}
          onClick={() => clickable && onClick(i)}
        />
      );
    }
    return stars;
  };

  if (!product) return <div>Loading...</div>;

  return (
    <div className="container mt-5">
      <button className="btn btn-secondary mb-3" onClick={() => navigate(-1)}>Back</button>
      <div className="row">
        <div className="col-md-6">
          <img src={`https://localhost:7007/images/${product.image}`} alt={product.name} className="img-fluid" />
        </div>
        <div className="col-md-6">
          <h2>{product.name}</h2>
          <p className="lead">{product.description}</p>
          <h4>${product.unitPrice?.toFixed(2)}</h4>
          <p className={`fw-semibold ${getStockClass(product.quantity)}`}>
            {getStockMessage(product.quantity)}
          </p>
          <button className="btn btn-primary" disabled={product.quantity === 0} onClick={handleAddToCart}>Add to Cart</button>
        </div>
      </div>

      {notification && (
        <div className="alert alert-info mt-3">
          {notification}
        </div>
      )}

      <hr className="my-5" />
      <h4 className="mb-3">Customer Reviews</h4>

      <div className="mb-3 d-flex align-items-center">
        <div>{renderStars(product.rating.averageRating)}</div>
        <div className="ms-2">({product.rating.totalReviews} reviews)</div>
      </div>

      <button className="btn btn-outline-primary mb-3" onClick={() => setShowModal(true)}>
        Add Review
      </button>

      {product.reviews.length === 0 ? (
        <p className="text-muted">No reviews yet.</p>
      ) : (
        <div className="list-group">
          {product.reviews.map((review) => (
            <div key={review.id} className="list-group-item">
              <div className="d-flex justify-content-between">
                <strong>{review.username}</strong>
                <small className="text-muted">{new Date(review.createdAt).toLocaleDateString()}</small>
              </div>
              <div className="mb-1">{renderStars(review.rating)}</div>
              {review.comment && <p>{review.comment}</p>}
            </div>
          ))}
        </div>
      )}

      {showModal && (
        <div className="modal d-block" tabIndex="-1" onClick={() => setShowModal(false)}>
          <div className="modal-dialog" onClick={(e) => e.stopPropagation()}>
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Add a Review</h5>
                <button type="button" className="btn-close" onClick={() => setShowModal(false)}></button>
              </div>
              <div className="modal-body">
                <div className="mb-3">
                  <label className="form-label">Rating</label>
                  <div>{renderStars(reviewRating, true, setReviewRating)}</div>
                </div>
                <div className="mb-3">
                  <label className="form-label">Comment</label>
                  <textarea
                    className="form-control"
                    value={reviewComment}
                    onChange={(e) => setReviewComment(e.target.value)}
                    rows="3"
                    placeholder="Write your review..."
                  />
                </div>
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" onClick={() => setShowModal(false)}>Cancel</button>
                <button className="btn btn-primary" onClick={handleReviewSubmit}>Submit Review</button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProductPage;
