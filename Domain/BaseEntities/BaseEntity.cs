namespace Domain.BaseEntities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int? UpdatedBy { get; set; }
    public int CreatedBy { get; set; }
    public int? DeletedBy { get; set; }
    public DateTime CreatedDate { get; set; } //bezen CreatedAt, UpdatedAt kimi de yazilir
    public DateTime UpdatedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDeleted { get; set; } //softDelete

    public BaseEntity()
    {
        CreatedDate = DateTime.Now;
    }
}
