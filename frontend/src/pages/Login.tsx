import React, { useState } from "react";
import { api } from "../api/api";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export default function Login() {
  const [username, setUsername] = useState("demo");
  const [password, setPassword] = useState("password");
  const [err, setErr] = useState("");
  const nav = useNavigate();
  const { login } = useAuth();

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    try {
      const res = await api.post("/auth/login", { username, password });
      login(res.data.token);
      nav("/dashboard");
    } catch (error: any) {
      setErr("Login failed");
    }
  }

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded shadow">
      <h2 className="text-xl font-semibold mb-4">Login</h2>
      {err && <div className="text-red-600 mb-2">{err}</div>}
      <form onSubmit={submit} className="space-y-3">
        <input value={username} onChange={e=>setUsername(e.target.value)} required className="w-full p-2 border" placeholder="Username" />
        <input value={password} onChange={e=>setPassword(e.target.value)} required className="w-full p-2 border" placeholder="Password" />
        <button className="w-full py-2 bg-blue-600 text-white rounded">Login</button>
      </form>
    </div>
  );
}
