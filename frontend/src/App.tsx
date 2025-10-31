import React from "react";
import { Outlet, Link } from "react-router-dom";
import { useAuth } from "./auth/AuthContext";

export default function App() {
  const { token, logout } = useAuth();
  return (
    <div>
      <header className="bg-white shadow p-4">
        <div className="container mx-auto flex justify-between">
          <Link to="/" className="text-xl font-bold">Pathlock</Link>
          <nav>
            { token ? (
              <button onClick={logout} className="text-sm px-3 py-1 bg-red-500 text-white rounded">Logout</button>
            ) : (
              <div className="space-x-2">
                <Link to="/login">Login</Link>
                <Link to="/register">Register</Link>
              </div>
            )}
          </nav>
        </div>
      </header>
      <main className="container mx-auto p-4">
        <Outlet />
      </main>
    </div>
  );
}
