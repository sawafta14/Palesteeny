using Palesteeny_Project.Models;

namespace Palesteeny_Project.ViewModels
{
    public class QusiViewMode
    {
        public Qusi Qusi { get; set; } = null!; // قصة كاملة
        public string? UserId { get; set; }     // معرف المستخدم (يمكن أن يكون null إذا لم يسجل دخول)
        public bool IsFavorite { get; set; }    // هل القصة مفضلة للمستخدم؟
        public int BookmarkOrder { get; set; }  // رقم الصورة التي تم وضع بوك مارك عليها (0 إذا لا يوجد)

    }
}
