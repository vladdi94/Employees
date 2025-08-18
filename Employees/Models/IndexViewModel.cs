using Employees.Models.Database;

namespace Employees.Models
{
    public class IndexViewModel(IEnumerable<EmployeeModel>? employees, PageViewModel? pageViewModel)
    {
        public IEnumerable<EmployeeModel>? Employees { get; } = employees;

        public PageViewModel? PageViewModel { get; } = pageViewModel;

    }
}
