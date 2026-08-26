using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> GetAllDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department?> GetDepartmentById(int id)
        {
            return await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Department> AddDepartment(Department newDepartment)
        {
            if (string.IsNullOrWhiteSpace(newDepartment.Name))
            {
                throw new ArgumentException("Department name is required.");
            }

            var nameExists = await _context.Departments
                .AnyAsync(d => d.Name.ToLower() == newDepartment.Name.ToLower());

            if (nameExists)
            {
                throw new ArgumentException($"A department named '{newDepartment.Name}' already exists.");
            }

            _context.Departments.Add(newDepartment);
            await _context.SaveChangesAsync();
            return newDepartment;
        }

        public async Task<Department?> UpdateDepartment(int id, Department updatedDepartment)
        {
            var existingDepartment = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (existingDepartment == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(updatedDepartment.Name))
            {
                throw new ArgumentException("Department name is required.");
            }

            var nameExists = await _context.Departments
                .AnyAsync(d => d.Id != id && d.Name.ToLower() == updatedDepartment.Name.ToLower());

            if (nameExists)
            {
                throw new ArgumentException($"A department named '{updatedDepartment.Name}' already exists.");
            }

            existingDepartment.Name = updatedDepartment.Name;
            await _context.SaveChangesAsync();
            return existingDepartment;
        }

        public async Task<bool> DeleteDepartment(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
            if (department == null)
            {
                return false;
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
