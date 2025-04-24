import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const RegisterPage = () => {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        confirmPassword: '',
        email: '',
        phone: '',
        firstName: '',
        name: ''
    });
    const [errors, setErrors] = useState({});
    const navigate = useNavigate();

    const validate = () => {
        const newErrors = {};

        if (formData.username.length < 4) {
            newErrors.username = 'Username must be at least 4 characters long.';
        }
        
        if (formData.password.length < 6) {
            newErrors.password = 'Password must be at least 6 characters long.';
        }
        
        if (formData.password !== formData.confirmPassword) {
            newErrors.confirmPassword = 'Passwords do not match.';
        }
        
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(formData.email)) {
            newErrors.email = 'Invalid email format.';
        }
        
        const phoneRegex = /^\+?[0-9]{7,15}$/;
        if (!phoneRegex.test(formData.phone)) {
            newErrors.phone = 'Invalid phone number.';
        }
        
        if (!formData.firstName.trim()) {
            newErrors.firstName = 'First name is required.';
        }
        
        if (!formData.name.trim()) {
            newErrors.name = 'Last name is required.';
        }
        
        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (e) => {
        setFormData({ ...formData, [e.target.name]: e.target.value });
    };

    const handleRegister = async (e) => {
        e.preventDefault();
        if (!validate()) return;
    
        try {
            const response = await fetch('https://localhost:7007/api/user', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData),
            });
    
            const errorData = await response.json();
            console.log("Response Status:", response.status);
            console.log("Response Data:", errorData);
    
            if (response.ok) {
                navigate('/');
            } else if (response.status === 409) {
                setErrors(prevErrors => ({
                    ...prevErrors,
                    username: errorData.message || "Username or email is already taken.",
                }));
            } else {
                setErrors({ server: errorData.message || 'Registration failed.' });
            }
        } catch (error) {
            console.error("Network Error:", error);
            setErrors({ server: 'Network error: ' + error.message });
        }
    };
    

    return (
        <div className="container mt-5">
            <h2 className="text-center">Register</h2>
            {errors.server && <p className="text-danger text-center">{errors.server}</p>}
            <form onSubmit={handleRegister} className="w-50 mx-auto">
                <div className="mb-3">
                    <label className="form-label">Username:</label>
                    <input
                        type="text"
                        className="form-control"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        required
                    />
                    {errors.username && <p className="text-danger">{errors.username}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Password:</label>
                    <input
                        type="password"
                        className="form-control"
                        name="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                    />
                    {errors.password && <p className="text-danger">{errors.password}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Confirm Password:</label>
                    <input
                        type="password"
                        className="form-control"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        required
                    />
                    {errors.confirmPassword && <p className="text-danger">{errors.confirmPassword}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Email:</label>
                    <input
                        type="email"
                        className="form-control"
                        name="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                    />
                    {errors.email && <p className="text-danger">{errors.email}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Phone:</label>
                    <input
                        type="text"
                        className="form-control"
                        name="phone"
                        value={formData.phone}
                        onChange={handleChange}
                        required
                    />
                    {errors.phone && <p className="text-danger">{errors.phone}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">First Name:</label>
                    <input
                        type="text"
                        className="form-control"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleChange}
                        required
                    />
                    {errors.firstName && <p className="text-danger">{errors.firstName}</p>}
                </div>
                <div className="mb-3">
                    <label className="form-label">Last Name:</label>
                    <input
                        type="text"
                        className="form-control"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                    />
                    {errors.name && <p className="text-danger">{errors.name}</p>}
                </div>
                <button type="submit" className="btn btn-primary w-100">Register</button>
            </form>
        </div>
    );
};

export default RegisterPage;
