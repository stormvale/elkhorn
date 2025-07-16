import { Routes, Route } from "react-router-dom";
import { SecureRoute } from "./SecureRoute";
import routes from "./routes";

export default function AppRouter() {
  return (
    <Routes>
      {routes.map(({ path, element, requiresAuth, allowedRoles }) => (
        <Route key={path} path={path}
          element={ requiresAuth
            ? <SecureRoute allowedRoles={allowedRoles}>
                {element}
              </SecureRoute>
            : element
          }
        />
      ))}
    </Routes>
  );
}