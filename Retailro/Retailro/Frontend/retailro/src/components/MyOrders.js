import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const orderStatusMap = {
  0: "None",
  1: "Paid",
  2: "Shipping",
  3: "Completed",
  4: "Cancelled",
  5: "Valid",
  6: "Processing"
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
    <div className="container mt-5">
      <h2>My Orders</h2>
      {orders.length === 0 ? (
        <p>You have no orders yet.</p>
      ) : (
        orders.map((order) => (
          <div
            key={order.id}
            className="card mb-4"
            style={{ cursor: "pointer" }}
            onClick={() => navigate(`/order/${order.id}`)}
          >
            <div className="card-body">
              <h5 className="card-title">Order #{order.id}</h5>
              <p className="card-text">
                <strong>Placed on:</strong>{" "}
                {new Date(order.createdAt).toLocaleString()}
              </p>
              <p className="card-text">
                <strong>Status:</strong> {orderStatusMap[order.status] ?? "Unknown"}
              </p>
              <p className="card-text">
                <strong>Total:</strong> ${order.totalPrice.toFixed(2)}
              </p>
            </div>
          </div>
        ))
      )}
    </div>
  );
};

export default MyOrders;
