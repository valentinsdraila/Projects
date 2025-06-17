import { useLocation } from "react-router-dom";
import PaymentForm from "../components/PaymentForm";

export default function Checkout() {
  const { state } = useLocation();
  const { orderId, amount } = state || {};

  if (!orderId || !amount) {
    return <div className="container mt-5 text-danger">Error: Missing payment info</div>;
  }

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-8 col-lg-6">
          <div className="card shadow-sm border-0">
            <div className="card-header bg-primary text-white">
              <h4 className="mb-0">Checkout</h4>
            </div>
            <div className="card-body">
              <PaymentForm amount={amount} orderId={orderId} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
