using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Pathlock.Api.Models;

namespace Pathlock.Api.Services
{
    public class InMemoryRepository : IRepository
    {
        private readonly ConcurrentDictionary<Guid, User> _users = new();
        private readonly ConcurrentDictionary<Guid, Project> _projects = new();
        private readonly ConcurrentDictionary<Guid, TaskItem> _tasks = new();

        public InMemoryRepository()
        {
            // Add a demo user for convenience (password: password)
            var demo = new User { Username = "demo", PasswordHash = "password" };
            _users.TryAdd(demo.Id, demo);
        }

        public void AddUser(User user) => _users.TryAdd(user.Id, user);
        public User? GetUserByUsername(string username) => _users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        public User? GetUserById(Guid id) => _users.TryGetValue(id, out var u) ? u : null;

        public IEnumerable<Project> GetProjectsForUser(Guid userId) => _projects.Values.Where(p => p.OwnerId == userId);
        public Project? GetProject(Guid id) => _projects.TryGetValue(id, out var p) ? p : null;
        public void AddProject(Project project) => _projects.TryAdd(project.Id, project);
        public void DeleteProject(Guid id)
        {
            if (_projects.TryRemove(id, out _))
            {
                // delete tasks under project
                var toDel = _tasks.Values.Where(t => t.ProjectId == id).Select(t => t.Id).ToArray();
                foreach (var tid in toDel) _tasks.TryRemove(tid, out _);
            }
        }

        public IEnumerable<TaskItem> GetTasksByProject(Guid projectId) => _tasks.Values.Where(t => t.ProjectId == projectId);
        public TaskItem? GetTask(Guid id) => _tasks.TryGetValue(id, out var t) ? t : null;
        public void AddTask(TaskItem task) => _tasks.TryAdd(task.Id, task);
        public void UpdateTask(TaskItem task) => _tasks.AddOrUpdate(task.Id, task, (_, __) => task);
        public void DeleteTask(Guid id) => _tasks.TryRemove(id, out _);
    }
}
