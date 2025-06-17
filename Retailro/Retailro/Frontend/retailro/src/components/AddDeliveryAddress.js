import React, { useEffect, useState } from "react";
import regions from "../data/regions.json";

const AddDeliveryAddress = ({ onClose, onAddressAdded }) => {
  const [fullName, setFullName] = useState("");
  const [address, setAddress] = useState("");
  const [counties, setCounties] = useState([]);
  const [cities, setCities] = useState([]);
  const [selectedCounty, setSelectedCounty] = useState("");
  const [selectedCity, setSelectedCity] = useState("");
  const [zipCode, setZipCode] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    setCounties(Object.keys(regions));
  }, []);

  useEffect(() => {
    if (selectedCounty && regions[selectedCounty]) {
      setCities(regions[selectedCounty]);
      setSelectedCity("");
    } else {
      setCities([]);
    }
  }, [selectedCounty]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!fullName || !address || !selectedCounty || !selectedCity) {
      setError("All fields are required!");
      return;
    }

    setError("");
    setLoading(true);

    const newAddress = {
      fullName,
      address,
      county: selectedCounty,
      city: selectedCity,
      zipCode
    };

    try {
      const response = await fetch("https://localhost:7007/api/address", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        credentials: "include",
        body: JSON.stringify(newAddress),
      });

      if (!response.ok) {
        throw new Error("Failed to submit address.");
      }

      const savedAddress = await response.json();
      onAddressAdded(savedAddress);
    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal fade show d-block" tabIndex="-1" style={{ backgroundColor: "rgba(0,0,0,0.5)" }}>
      <div className="modal-dialog modal-dialog-centered">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Add Delivery Address</h5>
            <button type="button" className="btn-close" onClick={onClose}></button>
          </div>

          <div className="modal-body">
            {error && <div className="alert alert-danger">{error}</div>}

            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label className="form-label">Full Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={fullName}
                  onChange={(e) => setFullName(e.target.value)}
                  placeholder="Name"
                />
              </div>

              <div className="mb-3">
                <label className="form-label">Address</label>
                <input
                  type="text"
                  className="form-control"
                  value={address}
                  onChange={(e) => setAddress(e.target.value)}
                  placeholder="Street and house number"
                />
              </div>

              <div className="mb-3">
                <label className="form-label">County</label>
                <select
                  className="form-select"
                  value={selectedCounty}
                  onChange={(e) => setSelectedCounty(e.target.value)}
                >
                  <option value="">Select county</option>
                  {counties.map((county) => (
                    <option key={county} value={county}>
                      {county}
                    </option>
                  ))}
                </select>
              </div>

              <div className="mb-3">
                <label className="form-label">City</label>
                <select
                  className="form-select"
                  value={selectedCity}
                  onChange={(e) => {
                    const cityName = e.target.value;
                    setSelectedCity(cityName);
                    const cityData = cities.find(city => city.name === cityName);
                    setZipCode(cityData?.zip ?? "");
                  }}
                  disabled={!selectedCounty}
                >
                  <option value="">Select city</option>
                  {cities.map((city) => (
                    <option key={city.name} value={city.name}>
                      {city.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="d-flex justify-content-end">
                <button
                  type="button"
                  className="btn btn-secondary me-2"
                  onClick={onClose}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  className="btn btn-primary"
                  disabled={loading}
                >
                  {loading ? "Submitting..." : "Submit"}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AddDeliveryAddress;
