import PaymentForm from "../components/PaymentForm";

export default function Checkout() {
  return (
    <div>
      <h2>Checkout</h2>
      <PaymentForm amount={0.99} />
    </div>
  );
}
