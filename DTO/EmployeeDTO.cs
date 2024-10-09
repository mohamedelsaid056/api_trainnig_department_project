namespace department.DTO
{
    public class EmployeeDTO
    {
        public int id { get; set; }
        public string name { get; set; } = null!;
        public int age { get; set; }
        public string? DepartmentName { get; set; }
    }
}
