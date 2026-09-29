using Microsoft.EntityFrameworkCore;
using UniversityApi.Data;
using UniversityApi.Models;
using UniversityApi.Repositories.Interfaces;

namespace UniversityApi.Repositories.Implementations;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _context;

    public EnrollmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Enrollment?> GetByStudentAndCourseAsync(
        int studentId,
        int courseId)
    {
        return await _context.Enrollments
            .FirstOrDefaultAsync(e =>
                e.StudentId == studentId &&
                e.CourseId == courseId);
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(
        int studentId)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Enrollment> AddAsync(
        Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();

        return enrollment;
    }

    public async Task UpdateAsync(
        Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
    }
}