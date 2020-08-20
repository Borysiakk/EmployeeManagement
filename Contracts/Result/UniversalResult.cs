using System.Collections.Generic;

namespace EmployeeManagement.Contracts.Result
{
    public class UniversalResult
    {
        public bool Success { get; set;}
        public IEnumerable<string> Error { get; set; }
    }
}