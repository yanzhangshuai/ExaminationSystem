using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Model.Interfaces;

/// <summary>主键为int型的基类</summary>
public interface IEntityInt : IEntity<int>
{
}

/// <summary>主键为Guid醒的基类</summary>
public interface IEntityGuid : IEntity<Guid>
{
}

public interface IEntity<TKey>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public TKey Id { get; set; }
}