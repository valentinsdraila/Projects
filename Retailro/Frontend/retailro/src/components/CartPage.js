import { useEffect, useState } from "react";

const CartPage = () => {
  const [cartItems, setCartItems] = useState([]);

  useEffect(() => {
    fetch("https://localhost:7007/api/cart/products", {
      method: "GET",
      credentials: "include",
    })
      .then((response) => {
        if (!response.ok) throw new Error("Failed to fetch cart items");
        return response.json();
      })
      .then((data) => setCartItems(data))
      .catch((error) => console.error(error));
  }, []);

  const handleRemove = (productId) => {
    fetch(`https://localhost:7007/api/cart/products/${productId}`, {
      method: "DELETE",
      credentials: "include",
    })
      .then((response) => {
        if (!response.ok) throw new Error("Failed to remove product");
        return response.text();
      })
      .then(() => {
        setCartItems((prevItems) => prevItems.filter((item) => item.id !== productId));
      })
      .catch((error) => console.error(error));
  };

  return (
    <div className="container mt-5">
      <h2>Your Shopping Cart</h2>
      <div className="row mt-4">
        {cartItems.length === 0 ? (
          <p>Your cart is empty.</p>
        ) : (
          cartItems.map((item) => (
            <div key={item.id} className="col-md-3 mb-4">
              <div className="card h-100">
                <img src={`/images/${item.image}`} alt={item.name} className="img-fluid" />
                <div className="card-body">
                  <h5 className="card-title">{item.name}</h5>
                  <p className="card-text">
                    <strong>${item.unitPrice?.toFixed(2)}</strong>
                  </p>
                  <button
                    className="btn btn-danger"
                    onClick={() => handleRemove(item.id)}
                  >
                    Remove
                  </button>
                </div>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
};

export default CartPage;
