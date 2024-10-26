using Microsoft.Extensions.Configuration;
using Vis.VleadProcessV3.Models;
using Vis.VleadProcessV3.Repositories;

namespace Vis.VleadProcessV3.Services
{
    public class ErrorCategoryService
    {
        private readonly TableWork _tableWork;
        private readonly TableWork tow;
       
        public ErrorCategoryService( TableWork tableWork,TableWork tow)
        {
            
            _tableWork = tableWork;
            this.tow = tow;

        }
        public IEnumerable<ErrorCategory> GetAllErrorCategory()
        {
         
                return tow.ErrorCategoryRepository.GetAllVal(x => x.Department).Where(x => x.IsDeleted == false).ToList();
          
        }
        public IEnumerable<ErrorCategory> GetErrorCategory(int DepartmentId, string Description)
        {
         
                return tow.ErrorCategoryRepository.GetAllVal(x => x.Department).Where(x => x.DepartmentId == DepartmentId || x.Description == Description).ToList();
           
        }
        public ErrorCategory GetErrorCategoryDetails(int Id)
        {
           
                return tow.ErrorCategoryRepository.GetAllVal(x => x.Department).Where(x => x.Id == Id).FirstOrDefault();

        }
        public bool CreateErrorCategory(ErrorCategory errorCategoryDBO)
        {
            bool status = false;
            if (errorCategoryDBO != null)
            {
                try
                {
                    var errorCategory = new ErrorCategory();
                    errorCategory.DepartmentId = errorCategoryDBO.DepartmentId;
                    errorCategory.Description = errorCategoryDBO.Description;
                    errorCategory.CreatedBy = errorCategoryDBO.CreatedBy;
                    errorCategory.CreatedUtc = DateTime.UtcNow;
                    errorCategory.UpdatedUtc = DateTime.UtcNow;
                    errorCategory.IsDeleted = false;
                    _tableWork.ErrorCategoryRepository.Insert(errorCategory);
                    long dbstatus = _tableWork.SaveChanges();
                    status = dbstatus > 0;
                }
                catch (Exception e)
                {

                }
            }
            return status;
        }
        public bool UpdateErrorCategory(ErrorCategory errorCategory)
        {
            bool status;
            try
            {
              
                    var ErrorCategoryDetails = tow.ErrorCategoryRepository.GetSingle(x => x.Id == errorCategory.Id);
                    ErrorCategoryDetails.DepartmentId = errorCategory.DepartmentId;
                    ErrorCategoryDetails.Description = errorCategory.Description;
                    ErrorCategoryDetails.CreatedUtc = DateTime.UtcNow;
                    ErrorCategoryDetails.UpdatedUtc = DateTime.UtcNow;
                    ErrorCategoryDetails.UpdatedBy = errorCategory.UpdatedBy;
                    ErrorCategoryDetails.IsDeleted = false;
                    ErrorCategoryDetails.DepartmentId = errorCategory.DepartmentId;
                    ErrorCategoryDetails.Description = errorCategory.Description;
                    tow.ErrorCategoryRepository.Update(ErrorCategoryDetails);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
        public bool RemoveErrorCategory(int Id)
        {
            bool status = false;
            try
            {
            
                    var existingErrorCategories = tow.ErrorCategoryRepository.GetSingle(x => x.Id == Id);
                    existingErrorCategories.IsDeleted = true;
                    tow.ErrorCategoryRepository.Update(existingErrorCategories);
                    long dbstatus = tow.SaveChanges();
                    status = dbstatus > 0;
               
            }
            catch (Exception)
            {
                throw;
            }
            return status;
        }
    }
}
