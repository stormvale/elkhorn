import { Routes, Route } from "react-router-dom";
import { SecureRoute } from "./SecureRoute";
import sidebarRoutes from "./routes";
import Layout from "../layouts/core/MainLayout";
import AuthLanding from "../features/auth/AuthLanding";
import AuthRedirect from "../features/auth/AuthRedirect";

export default function AppRouter() {
  return (
    <Routes>
      {sidebarRoutes.map(({ path, element, requiresAuth, allowedRoles }) => (
        <Route key={path} path={path}
          element={
            requiresAuth ? (
              <Layout>
                <SecureRoute allowedRoles={allowedRoles}>
                  {element}
                </SecureRoute>
              </Layout>
            ) : (
              element // Login page without layout
            )
          }
        />
      ))}

      // these are routes that we don't want to appear in the sidebar
      <Route path="/" element={<AuthLanding />} />
      <Route path="/login" element={<AuthLanding />} />
      <Route path="/signin-oidc" element={<AuthRedirect />} />

    </Routes>
  );
}