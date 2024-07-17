using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.InitialDataFactory
{
    public abstract class AbstractDataFactory
    {
        public abstract Grade[] GetGradeData();
        public abstract IdentityRole[] GetIdentityRoleData();
    }
}
