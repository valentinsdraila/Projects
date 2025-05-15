import React, { useEffect, useState } from "react";
import regions from "../data/regions.json";
import '../styles/AddDeliveryAddress.css';

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
    <div className="modal-overlay">
      <div className="modal-content">
        <button className="close-btn" onClick={onClose}>X</button>
        <h2>Add Delivery Address</h2>

        {error && <p style={{ color: "red" }}>{error}</p>}

        <form onSubmit={handleSubmit}>
          <label>Name:</label>
          <input
            type="text"
            value={fullName}
            onChange={(e) => setFullName(e.target.value)}
            placeholder="Full name"
          />

          <label>Address:</label>
          <input
            type="text"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            placeholder="Street and house number"
          />

          <label>County:</label>
          <select
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

          <label>City:</label>
          <select
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


          <button type="submit" disabled={loading}>
            {loading ? "Submitting..." : "Submit"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default AddDeliveryAddress;
