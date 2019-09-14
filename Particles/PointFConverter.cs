using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Globalization;

namespace Particles
{
    internal class PointFConverter:TypeConverter
    {
        /// <summary>
		///         Определяет, если этот преобразователь выполнить преобразование объекта заданного исходного типа в собственный тип преобразователя.
		///       </summary>
		/// <param name="context">
		///           Контекст средства форматирования.
		///            Этот объект можно использовать для получения дополнительных сведений о среде, из которой вызывается данный преобразователь.
		///            Это может быть <see langword="null" />, поэтому всегда следует выполнять проверку.
		///            Кроме того, свойства объекта контекста могут также возвращать <see langword="null" />.
		///         </param>
		/// <param name="sourceType">
		///           Для преобразования из нужного типа.
		///         </param>
		/// <returns>
		///         <see langword="true" /> Если этот объект может выполнить преобразование; в противном случае — <see langword="false" />.
		///       </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /*/// <summary>
        ///         Возвращает значение, указывающее, может ли этот преобразователь преобразовать объект в заданный тип, используя контекст.
        ///       </summary>
        /// <param name="context">
        ///           <see cref="T:System.ComponentModel.ITypeDescriptorContext" /> Объект, предоставляющий контекст формата.
        ///         </param>
        /// <param name="destinationType">
        ///           Объект <see cref="T:System.Type" /> представляющий тип, который требуется преобразовать.
        ///         </param>
        /// <returns>
        ///         Имеет значение <see langword="true" />, если преобразователь может выполнить преобразование, в противном случае — значение <see langword="false" />.
        ///       </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }*/

        /// <summary>
        ///         Преобразует указанный объект в <see cref="T:System.Drawing.Point" /> объекта.
        ///       </summary>
        /// <param name="context">
        ///           Контекст средства форматирования.
        ///            Этот объект можно использовать для получения дополнительных сведений о среде, из которой вызывается данный преобразователь.
        ///            Это может быть <see langword="null" />, поэтому всегда следует выполнять проверку.
        ///            Кроме того, свойства объекта контекста могут также возвращать <see langword="null" />.
        ///         </param>
        /// <param name="culture">
        ///           Объект, содержащий сведения о культуре, такие как язык, календарь и культурные соглашения, связанные с определенной культуры.
        ///            Он создан на основе стандарта RFC 1766.
        ///         </param>
        /// <param name="value">
        ///           Преобразуемый объект.
        ///         </param>
        /// <returns>
        ///         Преобразованный объект.
        ///       </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///             Невозможно выполнить преобразование.
        ///           </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string text = value as string;
            if (text == null)
            {
                return base.ConvertFrom(context, culture, value);
            }
            string text2 = text.Trim();
            if (text2.Length == 0)
            {
                return null;
            }
            if (culture == null)
            {
                culture = CultureInfo.CurrentCulture;
            }
            char c = culture.TextInfo.ListSeparator[0];
            string[] array = text2.Split(new char[]
            {
                c
            });
            float[] array2 = new float[array.Length];
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = (float)converter.ConvertFromString(context, culture, array[i]);
            }
            if (array2.Length == 2)
            {
                return new PointF(array2[0], array2[1]);
            }
            throw new ArgumentException("convert error");
        }

        /// <summary>
        ///         Преобразует указанный объект в заданный тип.
        ///       </summary>
        /// <param name="context">
        ///           Контекст средства форматирования.
        ///            Этот объект можно использовать для получения дополнительных сведений о среде, из которой вызывается данный преобразователь.
        ///            Это может быть <see langword="null" />, поэтому всегда следует выполнять проверку.
        ///            Кроме того, свойства объекта контекста могут также возвращать <see langword="null" />.
        ///         </param>
        /// <param name="culture">
        ///           Объект, содержащий сведения о культуре, такие как язык, календарь и культурные соглашения, связанные с определенной культуры.
        ///            Он создан на основе стандарта RFC 1766.
        ///         </param>
        /// <param name="value">
        ///           Преобразуемый объект.
        ///         </param>
        /// <param name="destinationType">
        ///           Тип, в который требуется преобразовать объект.
        ///         </param>
        /// <returns>
        ///         Преобразованный объект.
        ///       </returns>
        /// <exception cref="T:System.NotSupportedException">
        ///             Невозможно выполнить преобразование.
        ///           </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if (value is PointF)
            {
                if (destinationType == typeof(string))
                {
                    PointF point = (PointF)value;
                    if (culture == null)
                    {
                        culture = CultureInfo.CurrentCulture;
                    }
                    string separator = culture.TextInfo.ListSeparator + " ";
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(float));
                    string[] array = new string[2];
                    int num = 0;
                    array[num++] = converter.ConvertToString(context, culture, point.X);
                    array[num++] = converter.ConvertToString(context, culture, point.Y);
                    return string.Join(separator, array);
                }
                if (destinationType == typeof(InstanceDescriptor))
                {
                    PointF point2 = (PointF)value;
                    ConstructorInfo constructor = typeof(PointF).GetConstructor(new Type[]
                    {
                        typeof(float),
                        typeof(float)
                    });
                    if (constructor != null)
                    {
                        return new InstanceDescriptor(constructor, new object[]
                        {
                            point2.X,
                            point2.Y
                        });
                    }
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        ///         Создает экземпляр этого типа, задавая набор значений свойств для объекта.
        ///       </summary>
        /// <param name="context">
        ///           Дескриптор типа, через который может быть предоставлен дополнительный контекст.
        ///         </param>
        /// <param name="propertyValues">
        ///           Словарь новых значений свойств.
        ///            Словарь содержит ряд пар имя значение, один для каждого свойства, возвращаемые из <see cref="M:System.Drawing.PointConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" />.
        ///         </param>
        /// <returns>
        ///         Вновь созданный объект или <see langword="null" /> Если объект не был создан.
        ///          Реализация по умолчанию возвращает значение <see langword="null" />.
        ///       </returns>
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            if (propertyValues == null)
            {
                throw new ArgumentNullException("propertyValues");
            }
            object obj = propertyValues["X"];
            object obj2 = propertyValues["Y"];
            if (obj == null || obj2 == null || !(obj is float) || !(obj2 is float))
            {
                throw null;// new ArgumentException(SR.GetString("PropertyValueInvalidEntry"));
            }
            return new PointF((float)obj, (float)obj2);
        }

        /// <summary>
        ///         Определяет, если при изменении значения этого объекта требуется вызов <see cref="M:System.Drawing.PointConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> для создания нового значения.
        ///       </summary>
        /// <param name="context">
        ///           A <see cref="T:System.ComponentModel.TypeDescriptor" /> через какой дополнительный контекст может быть предоставлен.
        ///         </param>
        /// <returns>
        ///         <see langword="true" /> Если <see cref="M:System.Drawing.PointConverter.CreateInstance(System.ComponentModel.ITypeDescriptorContext,System.Collections.IDictionary)" /> метод должен вызываться при изменении одного или нескольких свойств этого объекта, в противном случае — <see langword="false" />.
        ///       </returns>
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return !true;//WAT
        }

        /// <summary>
        ///         Получает набор свойств для этого типа.
        ///          По умолчанию тип не возвращает какие-либо свойства.
        ///       </summary>
        /// <param name="context">
        ///           Дескриптор типа, через который может быть предоставлен дополнительный контекст.
        ///         </param>
        /// <param name="value">
        ///           Значение объекта, чтобы получить свойства.
        ///         </param>
        /// <param name="attributes">
        ///           Массив <see cref="T:System.Attribute" /> объекты, которые описывают свойства.
        ///         </param>
        /// <returns>
        ///         Набор свойств, которые доступны для этого типа данных.
        ///          Если свойства не предоставляются, этот метод может вернуть <see langword="null" />.
        ///          Реализация по умолчанию всегда возвращает <see langword="null" />.
        ///       </returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value.GetType(), attributes);
            return properties//.Sort(new string[]{"X","Y"})
                ;
        }

        /// <summary>
        ///         Определяет, поддерживает ли объект свойства.
        ///          По умолчанию это <see langword="false" />.
        ///       </summary>
        /// <param name="context">
        ///           A <see cref="T:System.ComponentModel.TypeDescriptor" /> через какой дополнительный контекст может быть предоставлен.
        ///         </param>
        /// <returns>
        ///         Имеет значение <see langword="true" />, если для поиска свойств этого объекта следует вызвать метод <see cref="M:System.Drawing.PointConverter.GetProperties(System.ComponentModel.ITypeDescriptorContext,System.Object,System.Attribute[])" />, в противном случае — значение <see langword="false" />.
        ///       </returns>
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}