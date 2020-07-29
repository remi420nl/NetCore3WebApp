using NoviSDP2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoviSDP2.Interface
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAll();
        Item GetById(int id);
        void Create(Item item);
        string GetType(int id);
        IEnumerable<Item> GetByEmployee(int employeeId);
        void SavePhotoUrl(int itemId, string relativePath);
        void HoldItem(Item item, Student student, int days);
        void Delete(int id);

    }
}
