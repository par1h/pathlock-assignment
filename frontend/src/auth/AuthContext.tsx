import React, { createContext, useContext, useState, useEffect } from "react";
import { setToken } from "../api/api";

type AuthCtx = {
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
};

const AuthContext = createContext<AuthCtx | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [token, setTokenState] = useState<string | null>(() => localStorage.getItem("token"));

  useEffect(() => {
    setToken(token);
    if (token) localStorage.setItem("token", token); else localStorage.removeItem("token");
  }, [token]);

  function login(t: string) { setTokenState(t); }
  function logout() { setTokenState(null); }

  return <AuthContext.Provider value={{ token, login, logout }}>{children}</AuthContext.Provider>;
};

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error("useAuth must be used inside AuthProvider");
  return ctx;
}
