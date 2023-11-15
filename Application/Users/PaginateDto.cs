namespace Application.Users
{
    public class PaginateDto
    {
        public int Page { get; set; }
        public int Size { get; set; }

        // Deconstructing assignment
        public void Deconstruct(out int page, out int size) => (page, size) = (Page, Size);
    }
}
