import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/LoginPage';
import HomePage from './components/HomePage';
import CartPage from './components/CartPage';
import RegisterPage from './components/RegisterPage';
import ProductPage from './components/ProductPage';
import MyOrders from './components/MyOrders';
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min";
import './App.css';
import OrderPage from './components/OrderPage';
import AddProductPage from './components/AddProductPage';
import PaymentForm from './components/PaymentForm';
import Checkout from './components/Checkout';


const App = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<LoginPage />} />
                <Route path="/home" element={<HomePage />} />
                <Route path="/cart" element={<CartPage />} />
                <Route path="/register" element={<RegisterPage/>}/>
                <Route path="/products/:productId" element={<ProductPage/>}/>
                <Route path="/myorders" element={<MyOrders/>}/>
                <Route path="/order/:orderId" element={<OrderPage/>}/>
                <Route path="/products/add" element={<AddProductPage/>}/>
                <Route path="/checkout" element={<Checkout/>}/>
            </Routes>
        </Router>
    );
};

export default App;
