import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
const CartPage = () => {
  const [cartItems, setCartItems] = useState([]);
  const navigate = useNavigate();


  useEffect(() => {
    fetch("https://localhost:7007/api/cart/products", {
      method: "GET",
      credentials: "include",
    })
      .then((response) => {
        if (!response.ok) throw new Error("Failed to fetch cart items");
        return response.json();
      })
      .then((data) =>
        setCartItems(
          data.map((item) => ({
            ...item,
            totalPrice: Number(item.totalPrice ?? 0),
            quantity: Number(item.quantity ?? 1),
            price: Number(item.totalPrice/item.quantity ?? 0),
          }))
        )
      )
      
      .catch((error) => console.error(error));
  }, []);

  const updateQuantity = (productId, delta) => {
    setCartItems((prevItems) =>
      prevItems.map((item) => {
        if (item.id !== productId) return item;
  
        const newQuantity = Math.max(1, (item.quantity ?? 1) + delta);
  
        return {
          ...item,
          quantity: newQuantity,
          totalPrice: item.price * newQuantity,
        };
      })
    );
  };

  const handleNextStep = () => {
  navigate("/cart/checkout", {
    state: {
      cartItems
    }
  });
};

  

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

  const totalCartPrice = cartItems.reduce(
    (sum, item) => sum + item.totalPrice,
    0
  ).toFixed(2);

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
                <img src={`https://localhost:7007/images/${item.image}`} alt={item.name} className="img-fluid" />
                <div className="card-body">
                  <h5 className="card-title">{item.name}</h5>
                  <div className="d-flex align-items-center mb-2">
                    <button
                      className="btn btn-sm btn-outline-secondary me-2"
                      onClick={() => updateQuantity(item.id, -1)}
                    >
                      −
                    </button>
                    <span>Quantity: {item.quantity}</span>
                    <button
                      className="btn btn-sm btn-outline-secondary ms-2"
                      onClick={() => updateQuantity(item.id, 1)}
                    >
                      +
                    </button>
                  </div>
                  <p className="card-text fw-bold">
                  Total: ${(item.price * item.quantity).toFixed(2)}
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

      {cartItems.length > 0 && (
        <>
          <hr />
          <div className="d-flex justify-content-between align-items-center">
          <p className="card-text fw-bold">Total: ${totalCartPrice}</p>
            <button className="btn btn-primary" onClick={handleNextStep}>
            Next Step
            </button>

          </div>
        </>
      )}
    </div>
  );
};

export default CartPage;
