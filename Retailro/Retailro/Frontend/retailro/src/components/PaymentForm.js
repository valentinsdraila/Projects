import { useEffect } from "react";
import dropin from "braintree-web-drop-in";

function PaymentForm({ amount }) {
  useEffect(() => {
    let dropinInstance;

    fetch("https://localhost:7085/api/braintree/client-token")
      .then((res) => res.json())
      .then((data) => {
        dropin.create(
          {
            authorization: data.clientToken,
            container: "#dropin-container"
          },
          (err, instance) => {
            if (err) {
              console.error(err);
              return;
            }
            dropinInstance = instance;

            document
              .getElementById("submit-button")
              .addEventListener("click", () => {
                instance.requestPaymentMethod((err, payload) => {
                  if (err) return console.error(err);

                  fetch("https://localhost:7085/api/braintree/checkout", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                      amount,
                      nonce: payload.nonce
                    })
                  })
                    .then((res) => res.json())
                    .then((result) => {
                      alert("Payment successful!");
                    })
                    .catch(console.error);
                });
              });
          }
        );
      });

    return () => {
      if (dropinInstance) {
        dropinInstance.teardown();
      }
    };
  }, [amount]);

  return (
    <div>
      <div id="dropin-container"></div>
      <button id="submit-button">Pay ${amount}</button>
    </div>
  );
}

export default PaymentForm;
