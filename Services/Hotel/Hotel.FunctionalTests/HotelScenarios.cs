using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Hotel.FunctionalTests
{
    public class HotelScenarios : HotelScenarioBase
    {
        [Fact]
        public async Task Get_get_all_hotels_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.HotelsList());

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_hotel_by_id_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.HotelById(1));

                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_hotel_by_id_and_response_bad_request_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.HotelById(int.MaxValue));

                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }

        [Fact]
        public async Task Get_get_hotel_by_name_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var response = await server.CreateClient()
                    .GetAsync(Get.ItemByName("Travel Lodge"));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}