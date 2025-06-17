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
                    .then((res) => {
                      if (!res.ok) {
                        return res.json().then(err => {
                          throw new Error(err.message || "Payment failed.");
                        });
                      }
                      return res.json();
                    })
                    .then(() => {
                      alert("Payment successful!");
                      navigate("/home");
                    })
                    .catch((err) => {
                      console.error("Payment error:", err);
                      alert(`${err}`);
                      navigate("/home");
                    });
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
    <>
      <div id="dropin-container" className="mb-4"></div>
      <button
        className="btn btn-primary w-100"
        id="submit-button"
      >
        Pay ${amount}
      </button>
    </>
  );
}

export default PaymentForm;
