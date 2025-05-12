import { useLocation } from "react-router-dom";
import PaymentForm from "../components/PaymentForm";

export default function Checkout() {
  const { state } = useLocation();
  const { orderId, amount } = state || {};

  if (!orderId || !amount) {
    return <p>Error: Missing payment info</p>;
  }

  return (
    <div>
      <h2>Checkout</h2>
      <PaymentForm amount={amount} orderId={orderId} />
    </div>
  );
}
