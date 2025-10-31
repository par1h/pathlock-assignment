import React, { useState } from "react";
import { api } from "../api/api";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const nav = useNavigate();
  const [err, setErr] = useState("");

  async function submit(e: React.FormEvent) {
    e.preventDefault();
    try {
      await api.post("/auth/register", { username, password });
      nav("/login");
    } catch {
      setErr("Register failed");
    }
  }

  return (
    <div className="max-w-md mx-auto bg-white p-6 rounded shadow">
      <h2 className="text-xl font-semibold mb-4">Register</h2>
      {err && <div className="text-red-600 mb-2">{err}</div>}
      <form onSubmit={submit} className="space-y-3">
        <input value={username} onChange={e=>setUsername(e.target.value)} required className="w-full p-2 border" placeholder="Username" />
        <input value={password} onChange={e=>setPassword(e.target.value)} required className="w-full p-2 border" placeholder="Password" />
        <button className="w-full py-2 bg-green-600 text-white rounded">Register</button>
      </form>
    </div>
  );
}
