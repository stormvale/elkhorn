import { Routes, Route } from 'react-router-dom';
import Register from '@/pages/Auth/Register';
import Login from '@/pages/Auth/Login';
import Home from '@/pages/Home/Home';
import RestaurantsPage from '@/features/restaurants/RestaurantsPage';
import Layout from '@/pages/Layout';

const Routing = () => {
  return (
    <Routes>
      <Route path="*" element={<Layout />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/design" element={<Home />} />
      <Route path="/restaurants" element={<RestaurantsPage />} />
    </Routes>
  );
};

export default Routing;
