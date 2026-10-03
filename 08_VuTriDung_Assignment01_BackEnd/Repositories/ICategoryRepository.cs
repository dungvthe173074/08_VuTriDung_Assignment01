using _08_VuTriDung_Assignment01.Models;

namespace _08_VuTriDung_Assignment01.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAll();
        Category? GetById(short id);
        Category Create(Category category);
        Category? Update(Category category);
        (bool Success, string? ErrorMessage) Delete(short id);
    }
}
