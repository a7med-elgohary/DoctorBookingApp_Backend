using Domain.Entities;
using Infrastructure.Repo.Interfaces;
using System;
using System.Numerics;
using System.Threading.Tasks;

public interface IUnitOfWork : IDisposable
{
    IGenericRepo<ApplicationUser> Users { get; }
    //IGenericRepo<Doctor> Doctors { get; }
    //IGenericRepo<Patient> Patients { get; }
    // أضف أي Repositories أخرى حسب Entities

    Task<int> SaveChangesAsync();
}
