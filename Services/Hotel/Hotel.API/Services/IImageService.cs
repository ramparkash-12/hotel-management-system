using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.API.Services
{
    public interface IImageService
    {
        Task Upload(IEnumerable<Model.Images> imageList);
    }
}