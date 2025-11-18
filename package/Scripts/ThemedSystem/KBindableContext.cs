namespace Khoai
{
    public static class KBindableContext
    {
        public static Reactive<float> alpha = new (0);
        public static Reactive<int> red = new (0);
        public static Reactive<string> text = new ("");
    }
}