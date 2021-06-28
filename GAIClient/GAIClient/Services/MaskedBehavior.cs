using System.Collections.Generic;
using Xamarin.Forms;

namespace GAIClient.Services
{
    public class MaskedBehavior : Behavior<Entry>
    {
        //private string _mask = "";
        //public string Mask
        //{
        //    get => _mask;
        //    set
        //    {
        //        _mask = value;
        //        SetPositions();
        //    }
        //}

        public static BindableProperty TextMaskProperty =
            BindableProperty.Create(nameof(TextMask), typeof(string), typeof(MaskedBehavior), null,
                BindingMode.TwoWay);

        public string TextMask
        {
            get => (string)GetValue(TextMaskProperty);
            set
            {
                SetValue(TextMaskProperty, value);
                SetPositions();
            }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        IDictionary<int, char> _positions;

        void SetPositions()
        {
            if (string.IsNullOrEmpty(TextMask))
            {
                _positions = null;
                return;
            }

            var list = new Dictionary<int, char>();
            for (var i = 0; i < TextMask.Length; i++)
                if (TextMask[i] != 'X')
                    list.Add(i, TextMask[i]);

            _positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) || _positions == null)
                return;

            if (text.Length > TextMask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in _positions)
                if (text.Length >= position.Key + 1)
                {
                    var value = position.Value.ToString();
                    if (text.Substring(position.Key, 1) != value)
                        text = text.Insert(position.Key, value);
                }

            if (entry.Text != text)
                entry.Text = text;
        }
    }
}