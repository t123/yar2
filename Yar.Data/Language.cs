using System;
using System.Collections.Generic;
using System.Linq;

namespace Yar.Data
{
    public class Language
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual User User { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        public virtual string RegEx { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual IList<Text> Texts { get; set; }
        public virtual IList<Option> Options { get; set; }

        private Option FindOption(string key)
        {
            return Options.SingleOrDefault(opt => opt.Key.ToLower().Trim() == key.ToLower().Trim());
        }

        public virtual T GetOption<T>(string key)
        {
            return GetOption(key, default(T));
        }

        public virtual T GetOption<T>(string key, T defaultValue)
        {
            if (Options == null)
            {
                return defaultValue;
            }

            var option = FindOption(key);

            if (option?.Value == null)
            {
                return defaultValue;
            }

            var value = (T)Convert.ChangeType(option.Value, typeof(T));

            if (value == null)
            {
                return defaultValue;
            }

            return value;
        }

        public virtual void SetOption(string key, object value)
        {
            if (Options == null)
            {
                Options = new List<Option>();
            }

            var option = FindOption(key);

            if (option == null)
            {
                option = new Option
                {
                    Key = key,
                    Language = this,
                    Value = value?.ToString() ?? "",
                    ExpectedType = value?.GetType().FullName ?? ""
                };

                Options.Add(option);
            }
            else
            {
                option.Value = value?.ToString() ?? "";
                option.ExpectedType = value?.GetType().FullName ?? "";
            }
        }
    }
}
