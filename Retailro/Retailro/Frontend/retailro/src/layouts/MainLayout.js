import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import { Outlet } from 'react-router-dom';

const MainLayout = ({ children }) => {
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
   useEffect(() => {
      fetch("https://localhost:7007/api/user/me", {
        method: "GET",
        credentials: "include"
      })
        .then(response => {
          if (!response.ok) throw new Error("Failed to fetch username");
          return response.json();
        })
        .then(data => setUsername(data.username))
        .catch(error => console.error(error));
    }, []);

  const handleLogout = async () => {
    try {
      const response = await fetch('https://localhost:7007/api/auth/logout', {
        method: 'POST',
        credentials: 'include',
      });
      if (response.ok) {
        window.location.href = "/";
      } else {
        console.error('Logout failed');
      }
    } catch (error) {
      console.error('Logout error:', error);
    }
  };

  const handleSearch = (query) => {
    navigate(`/search?query=${encodeURIComponent(query)}`);
  };

  return (
    <>
      <Header username={username} onLogout={handleLogout} onSearch={handleSearch} />
      <main className="container mt-4">
        <Outlet/>
      </main>
    </>
  );
};

export default MainLayout;
