﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Logic.Core
{
    public interface IProjectRepository
    {
        IEnumerable<Project> GetCreatorId(int creatorId);

        IList<Project> GetAllProjectsForUser(int userId);

        IEnumerable<Project> GetAll();

        Project GetById(int projectId);

        void Add(Project project);

        void Delete(int projectId);

        void Update(Project project);
    } 
}