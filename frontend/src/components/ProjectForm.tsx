import React, { useState } from "react";

export default function ProjectForm({ onCreate }: { onCreate:(title:string,desc?:string)=>void }) {
  const [title, setTitle] = useState("");
  const [desc, setDesc] = useState("");
  return (
    <form onSubmit={e=>{ e.preventDefault(); onCreate(title, desc); setTitle(""); setDesc(""); }}>
      <div className="flex gap-2">
        <input required value={title} onChange={e=>setTitle(e.target.value)} placeholder="New project title" className="flex-1 p-2 border rounded" />
        <input value={desc} onChange={e=>setDesc(e.target.value)} placeholder="Description (optional)" className="flex-1 p-2 border rounded" />
        <button className="px-3 py-2 bg-blue-600 text-white rounded">Add</button>
      </div>
    </form>
  );
}
