import { Routes, Route } from "react-router-dom";
import { SecureRoute } from "./SecureRoute";
import routes from "./routes";
import Layout from "../layouts/core/MainLayout";

export default function AppRouter() {
  return (
    <Routes>
      {routes.map(({ path, element, requiresAuth, allowedRoles }) => (
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
    </Routes>
  );
}