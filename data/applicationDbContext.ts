export interface ApplicationDbContext extends DbContext {
    categories: DbSet<category>;
    instructor: DbSet<Instructor>;
    course: DbSet<Course>;
}