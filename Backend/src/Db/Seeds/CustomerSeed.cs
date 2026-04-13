// using src.db.models;

// namespace src.db.seeds
// {
//     public static class CustomerSeed
//     {
//         public static List<CustomerModel> GetCustomers()
//         {
//             var customers = new List<CustomerModel>();

//             for (int i = 1; i <= 5; i++)
//             {
//                 customers.Add(new CustomerModel
//                 {
//                     Email = $"customer{i}@gmail.com",
//                     Password = $"customer{i}"
//                 });
//             }

//             return customers;
//         }
//     }
// }

using src.db.models;
 
namespace src.db.seeds
{
    public static class CustomerSeed
    {
        public static List<CustomerModel> GetCustomers()
        {
            // No hardcoded accounts - users must register via the signup page
            return new List<CustomerModel>();
        }
    }
}
 