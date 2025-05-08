using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRESTApi.Data;
using SimpleRESTApi.Models;

namespace SimpleRESTApi.data
{
    public class InstructorEF : IInstructor
    {
        private readonly ApplicationDbContext _context;
        public InstructorEF(ApplicationDbContext context)
        {
            _context = context;
        }
        public Instructor addInstructor(Instructor Instructor)
        {
            try
            {
                _context.Instructor.Add(Instructor);
                _context.SaveChanges();
                return Instructor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category: " + ex.Message);
            }
        }

        public void deleteInstructor(int InstructorID)
        {
            var instructor = _context.Instructor.FirstOrDefault(c => c.InstructorID == InstructorID);
            if (instructor == null)
            {
                throw new Exception("Instructor not found");
            }
            try
            {
                _context.Instructor.Remove(instructor);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting category: " + ex.Message);
            }
        }

        public Instructor GetInstructorById(int InstructorID)
        {
            var instructor = _context.Instructor.FirstOrDefault(c => c.InstructorID == InstructorID);
            if (instructor == null)
            {
                throw new Exception("Instructor not found");
            }
            return instructor;
        }

        public IEnumerable<Instructor> GetInstructors()
        {
           var instructor = _context.Instructor.OrderByDescending(c=>c.InstructorID).ToList();
            return instructor;
        }

        public Instructor updateInstructor(Instructor Instructor)
        {
            var existingInstructor = _context.Instructor.FirstOrDefault(c => c.InstructorID == Instructor.InstructorID);
            if (existingInstructor == null)
            {
                throw new Exception("Instructor not found");
            }
            try
            {
                existingInstructor.InstructorName = Instructor.InstructorName;
                existingInstructor.InstructorEmail = Instructor.InstructorEmail;
                existingInstructor.InstructorPhone = Instructor.InstructorPhone;
                existingInstructor.InstructorAddress = Instructor.InstructorAddress;
                existingInstructor.InstructorCity = Instructor.InstructorCity;
                _context.SaveChanges();
                return existingInstructor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating category: " + ex.Message);
            }
        }
    }
}