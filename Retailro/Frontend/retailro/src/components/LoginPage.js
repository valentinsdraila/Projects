import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; // Import useNavigate

const LoginPage = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate(); // Initialize useNavigate

const handleLogin = async (e) => {
    e.preventDefault();
    setError(''); // Clear any previous error

    try {
        const response = await fetch('https://localhost:7007/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username, password }),
            credentials: 'include', // Include credentials in the request
        });

        if (response.ok) {
            const data = await response.json();
            console.log('Login successful:', data);
            navigate('/home');
        } else {
            const errorData = await response.json();
            setError(errorData.message || 'Login failed');
        }
    } catch (error) {
        setError('Network error: ' + error.message);
    }
};


    return (
        <div className="container mt-5">
            <h2 className="text-center">Login</h2>
            <form onSubmit={handleLogin} className="w-50 mx-auto">
                <div className="mb-3">
                    <label className="form-label">Username:</label>
                    <input
                        type="text"
                        className="form-control"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                    />
                </div>
                <div className="mb-3">
                    <label className="form-label">Password:</label>
                    <input
                        type="password"
                        className="form-control"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                </div>
                <button type="submit" className="btn btn-primary w-100">Login</button>
                {error && <p className="text-danger">{error}</p>}
            </form>
        </div>
    );
};

export default LoginPage;
