namespace Onyrew.Projects.EventAccessHandler
{
    internal class Program
    {
        static void Main(string[] _args)
        {
            var handler = new EventAccessHandler();
            Console.WriteLine(handler.GetType().Name);
            // EventHandler`1

            var acT_value = handler.Create<string>("value");
            _ = acT_value ?? throw new InvalidOperationException();

            var ac_Value = handler.GetAccess<string>("value");

            ac_Value.Register(OnValueChanged1);
            ac_Value.Register(OnValueChanged2);
            acT_value.Invoke(new object(), null);
            // OnValueChanged1
            // OnValueChanged2

            ac_Value.Unregister(OnValueChanged2);
            acT_value.Invoke(new object(), null);
            // OnValueChanged1

            handler.Unregister<string>(OnValueChanged1);
            acT_value.Invoke(new object(), null);
            //

            var acT_e = handler.Create<int?>("e");
            _ = acT_e ?? throw new InvalidOperationException();

            var ac_E = acT_e.EventAccess;

            var e = (object _sender, int? _args) => Console.WriteLine($"e");
            ac_E += e;
            acT_e.Invoke(new object(), 1);
            // e

            ac_E -= e;
            acT_e.Invoke(new object(), 1);
            //
        }

        static void OnValueChanged1(object _sender, string? _args)
        {
            Console.WriteLine($"OnValueChanged1");
        }

        static void OnValueChanged2(object _sender, string? _args)
        {
            Console.WriteLine($"OnValueChanged2");
        }
    }
}