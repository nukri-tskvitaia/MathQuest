using Business.Models;

namespace Business.Validations
{
    public static class DataValidator
    {
        public static bool IsUserModelValid(UserModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.FirstName)
                || string.IsNullOrWhiteSpace(model.LastName) || (DateTime.Now.Year - model.BirthDate.Year) < 5
                || (DateTime.Now.Year - model.BirthDate.Year) > 105 
                || (model.Gender.ToLower() != "male" && model.Gender.ToLower() != "female"))
            {
                return false;
            }

            return true;
        }
    }
}
