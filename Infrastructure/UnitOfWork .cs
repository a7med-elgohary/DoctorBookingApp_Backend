using Domain.Entities;
using Infrastructure;
using Infrastructure.Repo;
using Infrastructure.Repo.Interfaces;
using System.Numerics;
using System.Threading.Tasks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    // Repositories مخزنة هنا
    private GenericRepo<ApplicationUser>? _users;
    //private GenericRepo<Doctor>? _doctors;
    //private GenericRepo<Patient>? _patients;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public IGenericRepo<ApplicationUser> Users => _users ??= new GenericRepo<ApplicationUser>(_db);
    //public IGenericRepo<Doctor> Doctors => _doctors ??= new GenericRepo<Doctor>(_db);
    //public IGenericRepo<Patient> Patients => _patients ??= new GenericRepo<Patient>(_db);

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public void Dispose()
    {
        _db.Dispose();
    }
}
