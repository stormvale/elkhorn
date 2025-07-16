import { Routes, Route } from 'react-router-dom';
import { Counter } from './features/counter/Counter';
import { Quotes } from './features/quotes/Quotes';
import { Restaurants } from './features/restaurants/Restaurants';
import TemplateTester from './features/theme/TemplateTester';
import LoginPage from './features/auth/LoginPage';
import HomePage from './features/home/HomePage';

export default function() {
  return (
    <Routes>
      <Route index element={<LoginPage />} />
      <Route path="/login" element={<LoginPage />} />

      <Route path="/home" element={<HomePage />} />
      <Route path="/counter" element={<Counter />} />
      <Route path="/quotes" element={<Quotes />} />
      <Route path="/restaurants" element={<Restaurants />} />
      <Route path="/theme" element={<TemplateTester />} />
    </Routes>
  );
};