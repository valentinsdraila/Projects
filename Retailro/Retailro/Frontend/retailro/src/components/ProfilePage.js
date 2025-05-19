import React, { useEffect, useState } from 'react';
import AddDeliveryAddress from '../components/AddDeliveryAddress';

const Button = ({ onClick, children }) => (
  <button
    onClick={onClick}
    className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
  >
    {children}
  </button>
);

const Card = ({ children }) => (
  <div className="border rounded-lg shadow-md bg-white">{children}</div>
);

const CardContent = ({ children, className = '' }) => (
  <div className={`p-4 ${className}`}>{children}</div>
);

const ProfilePage = () => {
  const [user, setUser] = useState(null);
  const [deliveryAddresses, setDeliveryAddresses] = useState([]);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const res = await fetch('https://localhost:7007/api/user', {
          credentials: 'include',
        });

        if (!res.ok) {
          throw new Error('Failed to fetch user data');
        }

        const data = await res.json();
        setUser(data);
        setDeliveryAddresses(data.deliveryAddresses || []);
      } catch (error) {
        console.error(error.message);
      }
    };

    fetchUser();
  }, []);

  const handleAddressAdded = (newAddress) => {
    setDeliveryAddresses((prev) => [...prev, newAddress]);
    setShowModal(false);
  };

  if (!user) return <div className="p-4">Loading...</div>;

  return (
    <div className="max-w-3xl mx-auto p-6 space-y-6">
      <h1 className="text-2xl font-bold mb-4">User Profile</h1>

      <Card>
        <CardContent>
          <div><strong>Username:</strong> {user.username}</div>
          <div><strong>Name:</strong> {user.firstName} {user.lastName}</div>
          <div><strong>Email:</strong> {user.email}</div>
          <div><strong>Phone:</strong> {user.phoneNumber}</div>
          <div><strong>Created At:</strong> {new Date(user.createdAt).toLocaleString()}</div>
        </CardContent>
      </Card>

      <div>
        <h2 className="text-xl font-semibold mb-2">Delivery Addresses</h2>

        {deliveryAddresses.length === 0 ? (
          <p>No addresses found.</p>
        ) : (
          <div className="space-y-4">
            {deliveryAddresses.map((address, index) => (
              <Card key={index}>
                <CardContent>
                  <div>{address.street || address.address}, {address.city}, {address.county}</div>
                  {address.notes && <div className="text-sm text-gray-500">{address.notes}</div>}
                </CardContent>
              </Card>
            ))}
          </div>
        )}

        <div className="mt-4">
          <button className="add-delivery-button" onClick={() => setShowModal(true)}>Add New Address</button>
        </div>

        {showModal && (
          <AddDeliveryAddress
            onClose={() => setShowModal(false)}
            onAddressAdded={handleAddressAdded}
          />
        )}
      </div>
    </div>
  );
};

export default ProfilePage;
