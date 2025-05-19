import { useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import AddDeliveryAddress from "./AddDeliveryAddress";

const CheckoutPage = () => {
  const { state } = useLocation();
  const navigate = useNavigate();
  const cartItems = state?.cartItems || [];

  const [addresses, setAddresses] = useState([]);
  const [selectedAddressId, setSelectedAddressId] = useState(null);
  const [showAddAddressModal, setShowAddAddressModal] = useState(false);

  useEffect(() => {
    fetch("https://localhost:7007/api/address", { credentials: "include" })
      .then(res => res.json())
      .then(setAddresses)
      .catch(console.error);
  }, []);

  const handlePlaceOrder = () => {
    const selectedAddress = addresses.find(a => a.id === selectedAddressId);
    if (!selectedAddress) return alert("Please select an address.");

    const productInfos = cartItems.map((item) => ({
      productId: item.id,
      name: item.name,
      quantityOrdered: item.quantity,
      priceAtPurchase: item.price.toFixed(2),
      image: item.image,
    }));

    fetch("https://localhost:7007/api/order", {
    method: "POST",
    credentials: "include",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(
        {productInfos, deliveryAddress: selectedAddress}),
  })
    .then((response) => {
      if (!response.ok) throw new Error("Order failed");
      return response.json();
    })
    .then(({ order }) => {
      const orderId = order.id;
      const amount = order.totalPrice;

    return fetch("https://localhost:7007/api/cart/clear", {
    method: "DELETE",
    credentials: "include",
  }).then(() => {
    navigate("/checkout", {
      state: { orderId, amount },
    });
  });
})
};

  return (
    <div className="container mt-4">
      <h2>Checkout</h2>

      <div className="mt-3">
        <h4>Select Delivery Address</h4>
        {addresses.length === 0 && <p>No addresses found.</p>}
        <div className="list-group">
          {addresses.map(addr => (
            <label key={addr.id} className="list-group-item">
              <input
                type="radio"
                name="address"
                checked={selectedAddressId === addr.id}
                onChange={() => setSelectedAddressId(addr.id)}
              />
              <span className="ms-2">{addr.fullName}, {addr.city}, {addr.address}</span>
            </label>
          ))}
        </div>
        <button
          className="btn btn-outline-secondary mt-2"
          onClick={() => setShowAddAddressModal(true)}
        >
          Add New Address
        </button>
        {showAddAddressModal && (
          <AddDeliveryAddress
            onClose={() => setShowAddAddressModal(false)}
            onAddressAdded={(newAddress) => {
              setAddresses((prev) => [...prev, newAddress]);
              setSelectedAddressId(newAddress.id);
              setShowAddAddressModal(false);
            }}
          />
        )}
      </div>

      <div className="mt-4">
        <h4>Payment Method</h4>
        <p>Payment will be completed after placing the order.</p>
      </div>

      <div className="mt-4">
        <button
          className="btn btn-primary"
          onClick={handlePlaceOrder}
          disabled={!selectedAddressId}
        >
          Place Order
        </button>
      </div>
    </div>
  );
};

export default CheckoutPage;
