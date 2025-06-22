import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

const orderStatusMap = {
  0: { label: "None", color: "#777" },
  1: { label: "Paid", color: "green" },
  2: { label: "Shipping", color: "orange" },
  3: { label: "Completed", color: "blue" },
  4: { label: "Cancelled", color: "red" },
};

const OrderPage = () => {
  const { orderId } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetch(`https://localhost:7007/api/order/${orderId}`, {
      method: "GET",
      credentials: "include",
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch order");
        return res.json();
      })
      .then((data) => {
        setOrder(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, [orderId]);

  if (loading) return <div className="container mt-5">Loading order...</div>;
  if (!order) return <div className="container mt-5">Order not found.</div>;

  const status = orderStatusMap[order.status] || { label: "Unknown", color: "#999" };

  return (
    <div
      className="container mt-5"
      style={{ fontFamily: "Segoe UI, Tahoma, Geneva, Verdana, sans-serif", color: "#222" }}
    >

      <div
        style={{
          display: "flex",
          gap: "40px",
          flexWrap: "wrap",
          fontSize: "1rem",
          marginBottom: "25px",
        }}
      >
        <div>
          <strong>Placed on:</strong>{" "}
          <span style={{ color: "#555" }}> {new Date(new Date(order.createdAt).getTime() + 3 * 60 * 60 * 1000).toLocaleString()}</span>
        </div>
        <div>
          <strong>Status:</strong>{" "}
          <span
            style={{
              backgroundColor: status.color,
              color: "white",
              padding: "4px 12px",
              borderRadius: "14px",
              fontWeight: "600",
              userSelect: "none",
            }}
          >
            {status.label}
          </span>
        </div>
        <div>
          <strong>Total:</strong>{" "}
          <span style={{ fontWeight: "700", color: "#111" }}>
            ${order.totalPrice.toFixed(2)}
          </span>
        </div>
      </div>

      <hr style={{ marginBottom: "30px" }} />

      <h4>Products</h4>
      {order.products?.length > 0 ? (
        <div
          className="row"
          style={{ display: "flex", gap: "20px", flexWrap: "wrap" }}
        >
          {order.products.map((product) => (
            <div
              key={product.id}
              onClick={() => navigate(`/products/${product.productId}`)}
              style={{
                cursor: "pointer",
                width: "calc(33% - 20px)",
                minWidth: "250px",
                borderRadius: "10px",
                boxShadow: "0 3px 8px rgba(0,0,0,0.1)",
                overflow: "hidden",
                backgroundColor: "#fff",
                transition: "transform 0.2s ease",
              }}
              onMouseEnter={(e) => (e.currentTarget.style.transform = "scale(1.03)")}
              onMouseLeave={(e) => (e.currentTarget.style.transform = "scale(1)")}
            >
              <div
                style={{
                  position: "relative",
                  width: "100%",
                  paddingTop: "75%",
                  overflow: "hidden",
                }}
              >
                <img
                  src={`https://localhost:7007/images/${product.image}`}
                  alt={product.name}
                  style={{
                    position: "absolute",
                    top: 0,
                    left: 0,
                    width: "100%",
                    height: "100%",
                    objectFit: "cover",
                    objectPosition: "center",
                  }}
                />
              </div>
              <div style={{ padding: "12px" }}>
                <h5 style={{ fontSize: "1.1rem", marginBottom: "8px", color: "#222" }}>
                  {product.name}
                </h5>
                <p style={{ fontSize: "0.9rem", color: "#555", marginBottom: "0" }}>
                  Price: ${product.priceAtPurchase.toFixed(2)} <br />
                  Quantity: {product.quantityOrdered}
                </p>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <p>No products found in this order.</p>
      )}
    </div>
  );
};

export default OrderPage;
