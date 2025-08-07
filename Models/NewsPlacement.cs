namespace Newfactjo.Models
{
    public enum NewsPlacement
    {
        None = 0,              // لا يظهر في مكان إضافي
        MainTop = 1,           // خبر رئيسي أول (كبير)
        Main = 2,              // خبر رئيسي صغير (الأول من الأربعة)
        TopBar = 3             // أخبار أعلى الموقع (شريط الأخبار المتحرك)
    }
}
