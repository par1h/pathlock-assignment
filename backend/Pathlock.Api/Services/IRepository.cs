using Pathlock.Api.Models;
using System;
using System.Collections.Generic;

namespace Pathlock.Api.Services
{
    public interface IRepository
    {
        // Users
        User? GetUserByUsername(string username);
        User? GetUserById(Guid id);
        void AddUser(User user);

        // Projects
        IEnumerable<Project> GetProjectsForUser(Guid userId);
        Project? GetProject(Guid id);
        void AddProject(Project project);
        void DeleteProject(Guid id);
        
        // Tasks
        IEnumerable<TaskItem> GetTasksByProject(Guid projectId);
        TaskItem? GetTask(Guid id);
        void AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
        void DeleteTask(Guid id);
    }
}
