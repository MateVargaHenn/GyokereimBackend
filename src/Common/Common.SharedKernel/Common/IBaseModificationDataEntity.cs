namespace Common.SharedKernel.Common;

public interface IBaseModificationDataEntity
{
    DateTime LastModified { get; set; }
    string ModifiedBy { get; set; }
}
