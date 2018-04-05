using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryTree.Data;
using StoryTree.Storage.DbContext;

namespace StoryTree.Storage
{
    public static class StorageHelper
    {
        public static ProjectEntity CreateProjectEntity(Project project)
        {
            return new ProjectEntity()
            {
                Name = project.Name,
                Description = project.Description,
                AssessmentSection = project.AssessmentSection,
                ProjectInformation = project.ProjectInformation,
                PersonEntity = new PersonEntity
                {
                    Name = project.ProjectLeader.Name,
                    Email = project.ProjectLeader.Email,
                    Telephone = project.ProjectLeader.Telephone,
                },
                ExpertEntities = new List<ExpertEntity>(project.Experts.Select(e => new ExpertEntity()
                {
                    PersonEntity = new PersonEntity()
                }))
                {

                }
            };
        }
    }
}
