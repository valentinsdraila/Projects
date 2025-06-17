import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const orderStatusMap = {
  0: { label: "None", color: "#777" },
  1: { label: "Paid", color: "green" },
  2: { label: "Shipping", color: "orange" },
  3: { label: "Completed", color: "blue" },
  4: { label: "Cancelled", color: "red" },
  5: { label: "Valid", color: "purple" },
  6: { label: "Processing", color: "teal" }
};

const MyOrders = () => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetch("https://localhost:7007/api/order", {
      method: "GET",
      credentials: "include",
    })
      .then((res) => {
        if (!res.ok) throw new Error("Failed to fetch orders");
        return res.json();
      })
      .then((data) => {
        setOrders(data);
        setLoading(false);
      })
      .catch((err) => {
        console.error(err);
        setLoading(false);
      });
  }, []);

  if (loading) return <div className="container mt-5">Loading orders...</div>;

  return (
    <div className="container mt-5" style={{ fontFamily: "Segoe UI, Tahoma, Geneva, Verdana, sans-serif" }}>
      <h2 style={{ marginBottom: "20px", textAlign: "center", color: "#333" }}>My Orders</h2>

      {orders.length === 0 ? (
        <p style={{ textAlign: "center", color: "#666" }}>You have no orders yet.</p>
      ) : (
        <div
          style={{
            display: "grid",
            gridTemplateColumns: "repeat(auto-fit, minmax(280px, 1fr))",
            gap: "20px"
          }}
        >
          {orders.map((order) => {
            const status = orderStatusMap[order.status] || { label: "Unknown", color: "#999" };
            return (
              <div
                key={order.id}
                className="card"
                style={{
                  cursor: "pointer",
                  borderRadius: "10px",
                  boxShadow: "0 4px 8px rgba(0,0,0,0.1)",
                  transition: "transform 0.15s ease-in-out",
                  padding: "15px",
                  backgroundColor: "#fff",
                  border: "1px solid #eee"
                }}
                onClick={() => navigate(`/order/${order.id}`)}
                onMouseEnter={e => e.currentTarget.style.transform = "scale(1.02)"}
                onMouseLeave={e => e.currentTarget.style.transform = "scale(1)"}
              >
                <h5 style={{ marginBottom: "10px", color: "#222" }}>Order #{order.orderNumber}</h5>
                <p style={{ marginBottom: "6px", fontSize: "0.9rem", color: "#555" }}>
                  <strong>Placed on:</strong> {new Date(order.createdAt).toLocaleString()}
                </p>
                <p style={{ marginBottom: "6px", fontSize: "0.9rem", color: "#555" }}>
                  <strong>Status:</strong>{" "}
                  <span
                    style={{
                      color: "#fff",
                      backgroundColor: status.color,
                      borderRadius: "12px",
                      padding: "2px 10px",
                      fontSize: "0.8rem",
                      fontWeight: "600",
                      userSelect: "none"
                    }}
                  >
                    {status.label}
                  </span>
                </p>
                <p style={{ marginTop: "12px", fontWeight: "700", fontSize: "1.1rem", color: "#111" }}>
                  Total: ${order.totalPrice.toFixed(2)}
                </p>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
};

export default MyOrders;
