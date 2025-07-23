import { Routes, Route } from "react-router-dom";
import { SecureRoute } from "./SecureRoute";
import routes from "./routes";
import Layout from "../layouts/core/MainLayout";
import SchoolSelector from "../features/auth/SchoolSelector";

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

      // these are routes that we don't want to appear in the sidebar
      <Route path="/school-selector" element={<SchoolSelector />} />

    </Routes>
  );
}