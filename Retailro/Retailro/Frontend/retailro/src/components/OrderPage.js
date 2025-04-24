import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const orderStatusMap = {
  0: "None",
  1: "Paid",
  2: "Shipping",
  3: "Completed",
  4: "Cancelled",
};

const OrderPage = () => {
  const { orderId } = useParams();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(true);

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

  return (
    <div className="container mt-5">
      <h2>Order #{order.id}</h2>
      <p>
        <strong>Placed on:</strong> {new Date(order.createdAt).toLocaleString()}
      </p>
      <p>
        <strong>Status:</strong> {orderStatusMap[order.status] ?? "Unknown"}
      </p>
      <p>
        <strong>Total:</strong> ${order.totalPrice.toFixed(2)}
      </p>
    </div>
  );
};

export default OrderPage;
