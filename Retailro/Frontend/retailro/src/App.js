import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LoginPage';
import HomePage from './components/HomePage';
import CartPage from './components/CartPage';
import RegisterPage from './components/RegisterPage';
import ProductPage from './components/ProductPage';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import './App.css';


const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/cart" element={<CartPage />} />
                <Route path="/register" element={<RegisterPage/>}/>
                <Route path="/products/:productId" element={<ProductPage/>}/>
            </Routes>
        </Router>
    );
};

export default App;
