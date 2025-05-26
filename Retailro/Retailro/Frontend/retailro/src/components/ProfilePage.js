import React, { useEffect, useState } from 'react';
import AddDeliveryAddress from '../components/AddDeliveryAddress';

const ProfilePage = () => {
  const [user, setUser] = useState(null);
  const [deliveryAddresses, setDeliveryAddresses] = useState([]);
  const [showModal, setShowModal] = useState(false);
  const [addressToDelete, setAddressToDelete] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const res = await fetch('https://localhost:7007/api/user', {
          credentials: 'include',
        });

        if (!res.ok) throw new Error('Failed to fetch user data');

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

  const handleEditUser = () => {
    alert('Edit user feature coming soon!');
  };

  const handleDeleteAddress = async (addressId) => {
    try {
      const res = await fetch(`https://localhost:7007/api/address/${addressId}`, {
        method: 'DELETE',
        credentials: 'include',
      });

      if (!res.ok) throw new Error('Failed to delete address');

      setDeliveryAddresses((prev) =>
        prev.filter((addr) => addr.id !== addressId)
      );
      setAddressToDelete(null);
    } catch (error) {
      console.error(error.message);
      alert('An error occurred while deleting the address.');
    }
  };

  if (!user) return <div className="p-4">Loading...</div>;

  return (
    <div className="container mt-5">
      <h1 className="mb-4">User Profile</h1>

      <div className="card mb-4 shadow-sm">
        <div className="card-body d-flex justify-content-between align-items-center">
          <div>
          <p><strong>Username:</strong> {user.username}</p>
          <p><strong>Name:</strong> {user.firstName} {user.lastName}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Phone:</strong> {user.phoneNumber}</p>
          <p><strong>Created At:</strong> {new Date(user.createdAt).toLocaleString()}</p>
          </div>
          <button className="btn btn-outline-primary btn-sm" onClick={handleEditUser}>
            <i className="bi bi-pencil"> Edit</i>
          </button>
        </div>
      </div>

      <div>
        <h2 className="h5 mb-3">Delivery Addresses</h2>

        {deliveryAddresses.length === 0 ? (
          <p>No addresses found.</p>
        ) : (
          <div className="row g-3">
            {deliveryAddresses.map((address) => (
              <div key={address.id} className="col-12">
                <div className="card shadow-sm">
                  <div className="card-body d-flex justify-content-between align-items-center">
                    <div>
                      <p className="mb-1">
                        {address.street || address.address}, {address.city}, {address.county}
                      </p>
                      {address.notes && (
                        <small className="text-muted">{address.notes}</small>
                      )}
                    </div>
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => setAddressToDelete(address.id)}
                    >
                      <i className="bi bi-trash"></i>
                    </button>
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}

        <div className="mt-4">
          <button className="btn btn-primary" onClick={() => setShowModal(true)}>
            Add New Address
          </button>
        </div>
      </div>

      {showModal && (
        <AddDeliveryAddress
          onClose={() => setShowModal(false)}
          onAddressAdded={handleAddressAdded}
        />
      )}

      {addressToDelete && (
        <div
          className="modal fade show d-block"
          tabIndex="-1"
          role="dialog"
          style={{ backgroundColor: 'rgba(0, 0, 0, 0.5)' }}
        >
          <div className="modal-dialog modal-dialog-centered" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Confirm Deletion</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={() => setAddressToDelete(null)}
                ></button>
              </div>
              <div className="modal-body">
                <p>Are you sure you want to delete this address?</p>
              </div>
              <div className="modal-footer">
                <button
                  type="button"
                  className="btn btn-secondary"
                  onClick={() => setAddressToDelete(null)}
                >
                  Cancel
                </button>
                <button
                  type="button"
                  className="btn btn-danger"
                  onClick={() => handleDeleteAddress(addressToDelete)}
                >
                  Delete
                </button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfilePage;
