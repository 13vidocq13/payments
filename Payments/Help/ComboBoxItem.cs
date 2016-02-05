namespace Payments.Help
{
    public class ComboBoxItem
    {
        public int Key { get; set; }
        public string Value { get; set; }
    
        public ComboBoxItem(){}
        public ComboBoxItem(int key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
