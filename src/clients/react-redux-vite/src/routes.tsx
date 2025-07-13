import { Routes, Route } from 'react-router-dom';
import { Counter } from './features/counter/Counter';
import { Quotes } from './features/quotes/Quotes';
import { Restaurants } from './features/restaurants/Restaurants';
import TemplateTester from './features/theme/TemplateTester';

export default function() {
  return (
    <Routes>
      <Route path="*" element={<Counter />} />
      <Route path="/quotes" element={<Quotes />} />
      <Route path="/restaurants" element={<Restaurants />} />
      <Route path="/theme" element={<TemplateTester />} />
    </Routes>
  );
};