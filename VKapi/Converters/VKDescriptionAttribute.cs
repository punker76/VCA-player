using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkontakteAPI.Converters
{
    // Сводка:
    //     Задает описание свойства или события.
    [AttributeUsage(AttributeTargets.All)]
    public class VKDescriptionAttribute : Attribute
    {
        private String description;
        // Сводка:
        //     Определяет значение по умолчанию для атрибута System.ComponentModel.DescriptionAttribute,
        //     являющееся пустой строкой (""). Это статическое (static) поле доступно только
        //     для чтения.
        public static readonly VKDescriptionAttribute Default = new VKDescriptionAttribute();

        // Сводка:
        //     Инициализирует новый экземпляр класса System.ComponentModel.DescriptionAttribute
        //     без параметров.
        public VKDescriptionAttribute()
        {
            this.description = String.Empty;
        }
        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.ComponentModel.DescriptionAttribute
        //     с указанным описанием.
        //
        // Параметры:
        //   description:
        //     Текст описания.
        public VKDescriptionAttribute(string description)
        {
            this.description = description;
        }

        // Сводка:
        //     Возвращает описание, хранящееся в данном атрибуте.
        //
        // Возвращает:
        //     Описание, хранящееся в данном атрибуте.
        public virtual string Description { get { return this.description; } }


        // Сводка:
        //     Возвращает значение, показывающее, равно ли значение данного объекта текущему
        //     атрибуту VkontakteAPI.Converters.DescriptionAttribute.
        //
        // Параметры:
        //   obj:
        //     Объект, проверяемый на равенство.
        //
        // Возвращает:
        //     Значение true, если значение данного объекта равно значению текущего; в противном
        //     случае — значение false.
        public override bool Equals(object obj)
        {
            VKDescriptionAttribute attr = obj as VKDescriptionAttribute;
            if (attr == null) return false;

            return this.Description.Equals(attr.Description, StringComparison.CurrentCulture);
        }
        //
        public override int GetHashCode()
        {
            return this.Description.GetHashCode();
        }
    }
}
