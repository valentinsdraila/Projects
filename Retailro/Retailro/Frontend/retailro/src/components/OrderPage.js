import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

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

      <h4 className="mt-4">Products</h4>
      {order.products?.length > 0 ? (
        <div className="row">
          {order.products.map((product) => (
            <div
              key={product.id}
              className="col-md-4 mb-3"
              onClick={() => navigate(`/products/${product.productId}`)}
              style={{ cursor: "pointer" }}
            >
              <div className="card h-100">
                <img
                  src={`https://localhost:7007/images/${product.image}`}
                  className="card-img-top"
                  alt={product.name}
                  style={{ objectFit: "cover", height: "200px" }}
                />
                <div className="card-body">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="card-text">
                    Price: ${product.priceAtPurchase.toFixed(2)} <br />
                    Quantity: {product.quantityOrdered}
                  </p>
                </div>
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
