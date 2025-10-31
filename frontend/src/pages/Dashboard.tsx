import React, { useEffect, useState } from "react";
import { api } from "../api/api";
import { Link } from "react-router-dom";
import ProjectForm from "../components/ProjectForm";

type Project = { id: string; title: string; description?: string };

export default function Dashboard() {
  const [projects, setProjects] = useState<Project[]>([]);

  async function load() {
    const res = await api.get("/projects");
    setProjects(res.data);
  }

  useEffect(() => { load(); }, []);

  async function handleCreate(title: string, description?: string) {
    await api.post("/projects", { title, description });
    load();
  }

  async function handleDelete(id: string) {
    await api.delete(`/projects/${id}`);
    load();
  }

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Projects</h1>
      <ProjectForm onCreate={handleCreate} />
      <div className="grid gap-3 mt-4">
        {projects.map(p => (
          <div key={p.id} className="p-4 bg-white rounded shadow flex justify-between items-center">
            <div>
              <Link to={`/projects/${p.id}`} className="font-semibold">{p.title}</Link>
              <div className="text-sm text-gray-600">{p.description}</div>
            </div>
            <div>
              <button onClick={()=>handleDelete(p.id)} className="px-3 py-1 bg-red-500 text-white rounded">Delete</button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
