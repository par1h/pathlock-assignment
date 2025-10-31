import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { api } from "../api/api";

type Task = { id: string; title: string; description?: string; isCompleted: boolean };

export default function ProjectDetails() {
  const { id } = useParams<{ id: string }>();
  const [project, setProject] = useState<any>(null);
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");

  async function load() {
    if (!id) return;
    const p = await api.get(`/projects/${id}`);
    setProject(p.data);
    const t = await api.get(`/projects/${id}/tasks`);
    setTasks(t.data);
  }

  useEffect(()=>{ load(); }, [id]);

  async function addTask() {
    if (!id) return;
    await api.post(`/projects/${id}/tasks`, { title, estimatedHours: 1 });
    setTitle("");
    load();
  }

  async function toggleComplete(task: Task) {
    await api.put(`/tasks/${task.id}`, { title: task.title, description: task.description, dueDate: null, isCompleted: !task.isCompleted, estimatedHours: 1 });
    load();
  }

  async function delTask(task:Task){
    await api.delete(`/tasks/${task.id}`);
    load();
  }

  async function schedule(){
    // Smart scheduler: call scheduler endpoint with tasks
    if (!id) return;
    const payload = { tasks: tasks.map(t => ({ title: t.title, estimatedHours: 1, dueDate: null })) };
    const res = await api.post(`/v1/projects/${id}/schedule`, payload);
    alert("Recommended order: " + JSON.stringify(res.data.recommendedOrder));
  }

  return (
    <div>
      <h1 className="text-xl font-bold">{project?.title}</h1>
      <p className="text-sm text-gray-600">{project?.description}</p>

      <div className="mt-4">
        <div className="flex gap-2">
          <input value={title} onChange={e=>setTitle(e.target.value)} placeholder="Task title" className="p-2 border rounded flex-1" />
          <button onClick={addTask} className="px-3 py-2 bg-green-600 text-white rounded">Add Task</button>
          <button onClick={schedule} className="px-3 py-2 bg-indigo-600 text-white rounded">Smart Schedule</button>
        </div>

        <div className="mt-4 space-y-2">
          {tasks.map(t => (
            <div key={t.id} className="p-3 bg-white rounded shadow flex justify-between">
              <div>
                <div className="font-medium">{t.title}</div>
                <div className="text-sm text-gray-600">{t.description}</div>
              </div>
              <div className="flex gap-2 items-center">
                <button onClick={()=>toggleComplete(t)} className="px-2 py-1 border rounded">{t.isCompleted ? "Mark Active" : "Complete"}</button>
                <button onClick={()=>delTask(t)} className="px-2 py-1 bg-red-500 text-white rounded">Delete</button>
              </div>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
