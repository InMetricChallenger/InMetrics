﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class BaseEntity
{
    protected BaseEntity(int id) => Id = id;

    protected BaseEntity()
    {
    }

    [Key]
    public int Id { get; protected set; }
}