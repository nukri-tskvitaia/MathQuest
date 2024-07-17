using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Data.Data.InitialDataFactory
{
    public class ReleaseDataFactory : AbstractDataFactory
    {
        public override Grade[] GetGradeData()
        {
            return
            [
                new Grade
                {
                    Id = 1,
                    GradeName = "1st Grade",
                    GradeLevel = 1
                },
                new Grade
                {
                    Id = 2,
                    GradeName = "2nd Grade",
                    GradeLevel = 2
                },
                new Grade
                {
                    Id = 3,
                    GradeName = "3rd Grade",
                    GradeLevel = 3
                },
                new Grade
                {
                    Id = 4,
                    GradeName = "4th Grade",
                    GradeLevel = 4
                },
                new Grade
                {
                    Id = 5,
                    GradeName = "5th Grade",
                    GradeLevel = 5
                },
                new Grade
                {
                    Id = 6,
                    GradeName = "6th Grade",
                    GradeLevel = 6
                },
                new Grade
                {
                    Id = 7,
                    GradeName = "7th Grade",
                    GradeLevel = 7
                },
                new Grade
                {
                    Id = 8,
                    GradeName = "8th Grade",
                    GradeLevel = 8
                },
                new Grade
                {
                    Id = 9,
                    GradeName = "9th Grade",
                    GradeLevel = 9
                },
                new Grade
                {
                    Id = 10,
                    GradeName = "10th Grade",
                    GradeLevel = 10
                },
                new Grade
                {
                    Id = 11,
                    GradeName = "11th Grade",
                    GradeLevel = 11
                },
                new Grade
                {
                    Id = 12,
                    GradeName = "12th Grade",
                    GradeLevel = 12
                }
            ];    
        }

        public override IdentityRole[] GetIdentityRoleData()
        {
            return
            [
                new IdentityRole
                {
                    Id = "1",
                    Name = "User"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Admin"
                }
            ];
        }
    }
}
