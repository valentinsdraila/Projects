import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import dropin from "braintree-web-drop-in";

function PaymentForm({ amount, orderId }) {
  const [dropinInstance, setDropinInstance] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    let instance;

    fetch("https://localhost:7007/api/braintree/client-token", {
      credentials: "include",
    })
      .then((res) => res.json())
      .then((data) => {
        dropin.create(
          {
            authorization: data.clientToken,
            container: "#dropin-container",
          },
          (err, createdInstance) => {
            if (err) {
              console.error(err);
              return;
            }
            instance = createdInstance;
            setDropinInstance(instance);

            document
              .getElementById("submit-button")
              .addEventListener("click", () => {
                instance.requestPaymentMethod((err, payload) => {
                  if (err) return console.error(err);

                  fetch("https://localhost:7007/api/braintree/checkout", {
                    method: "POST",
                    credentials: "include",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                      amount,
                      nonce: payload.nonce,
                      orderId,
                    }),
                  })
                    .then((res) => res.json())
                    .then((result) => {
                      alert("Payment successful!");
                    })
                    .then(() => navigate("/home"))
                    
                    .catch(console.error);
                });
              });
          }
        );
      });

    return () => {
      if (instance) {
        instance.teardown();
      }
    };
  }, [amount, orderId]);

  return (
    <div>
      <div id="dropin-container"></div>
      <button class="btn btn-outline-primary mt-2" id="submit-button">Pay ${amount}</button>
    </div>
  );
}

export default PaymentForm;
