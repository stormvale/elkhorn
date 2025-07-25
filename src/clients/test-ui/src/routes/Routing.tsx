import { Routes, Route } from 'react-router-dom';
import AuthLanding from '@/pages/Auth/AuthLanding';
import AuthRedirect from '@/pages/Auth/AuthRedirect';
import PostLoginHandler from '@/pages/Auth/PostLoginHandler';
import SchoolSelector from '@/pages/Auth/SchoolSelector';
import Home from '@/pages/Home/Home';
import ProtectedRoute from '@/components/ProtectedRoute';

const Routing = () => {
  return (
    <Routes>
      <Route path="/" element={<AuthLanding />} />
      <Route path="/register" element={<AuthLanding />} />
      <Route path="/login" element={<AuthLanding />} /> {/* All auth routes lead to the same unified page */}
      <Route path="/signin-oidc" element={<AuthRedirect />} />
      <Route path="/post-login" element={
        <ProtectedRoute>
          <PostLoginHandler />
        </ProtectedRoute>
      } />
      <Route path="/school-selector" element={
        <ProtectedRoute>
          <SchoolSelector />
        </ProtectedRoute>
      } />
      <Route path="/home" element={
        <ProtectedRoute>
          <Home />
        </ProtectedRoute>
      } />
    </Routes>
  );
};

export default Routing;
