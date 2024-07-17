﻿using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
