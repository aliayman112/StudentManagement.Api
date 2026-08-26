using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartments();
        Task<Department?> GetDepartmentById(int id);
        Task<Department> AddDepartment(Department newDepartment);
        Task<Department?> UpdateDepartment(int id, Department updatedDepartment);
        Task<bool> DeleteDepartment(int id);
    }
}
